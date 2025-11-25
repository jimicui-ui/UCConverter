# Frontend Release Guide

This guide provides step-by-step instructions for setting up and managing releases of the UCConverter frontend application using Azure DevOps release pipelines.

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Pipeline Setup](#pipeline-setup)
4. [Configuration](#configuration)
5. [Deployment Options](#deployment-options)
6. [Release Process](#release-process)
7. [Troubleshooting](#troubleshooting)

---

## Overview

The frontend release pipeline automates the build and deployment of the React + Vite application. The pipeline:
- Builds the React application with TypeScript
- Runs linting checks
- Creates production-ready artifacts
- Deploys to staging and production environments

---

## Prerequisites

### Required Access
- Azure DevOps project with appropriate permissions
- Access to create and manage release pipelines
- Access to deployment targets (Azure, servers, etc.)

### Required Tools
- Azure DevOps account
- Node.js 18.x or later (installed on build agents)
- npm or yarn package manager

### Required Information
- API base URL for production environment
- Deployment target credentials and configuration
- Environment-specific settings

---

## Pipeline Setup

### Step 1: Create a New Release Pipeline

1. Navigate to your Azure DevOps project
2. Go to **Pipelines** → **Releases**
3. Click **New pipeline** or **New** → **New release pipeline**
4. Select **Empty job** template

### Step 2: Add Pipeline YAML File

1. In your repository, the pipeline YAML file is located at:
   ```
   SolutionDotnetReact/frontend/azure-pipelines-release-frontend.yml
   ```

2. In Azure DevOps:
   - Go to **Pipelines** → **Pipelines**
   - Click **New pipeline**
   - Select your repository
   - Choose **Existing Azure Pipelines YAML file**
   - Select the branch and path: `SolutionDotnetReact/frontend/azure-pipelines-release-frontend.yml`

### Step 3: Configure Pipeline Variables

Create a variable group named `FrontendReleaseVariables`:

1. Go to **Pipelines** → **Library**
2. Click **+ Variable group**
3. Name it `FrontendReleaseVariables`
4. Add the following variables:

| Variable Name | Description | Example | Secret |
|--------------|-------------|---------|--------|
| `VITE_API_BASE_URL` | API base URL for production | `https://api.yourapp.com` | No |
| `deploymentTarget` | Target deployment method | `AzureStaticWebApps` | No |
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Azure Static Web Apps token | (token) | Yes |
| `storageAccountName` | Azure Storage account name | `mystorageaccount` | No |
| `storageAccountKey` | Azure Storage account key | (key) | Yes |
| `azureSubscription` | Azure subscription name | `My Subscription` | No |
| `webServerPath` | Linux web server path | `/var/www/html` | No |
| `deploymentTargetPath` | File copy target path | `\\server\share\frontend` | No |

**Note**: Mark sensitive values (tokens, keys, passwords) as **Secret** by checking the lock icon.

### Step 4: Create Environments

1. Go to **Pipelines** → **Environments**
2. Click **Create environment**
3. Create two environments:
   - **Staging**: For testing before production
   - **Production**: For live deployment

4. For each environment, configure:
   - **Approvals**: Add required approvers (recommended for Production)
   - **Checks**: Add any required checks (e.g., security scans)
   - **Deployment targets**: Add your deployment targets

---

## Configuration

### Build Configuration

The pipeline uses the following default settings:
- **Node.js version**: 18.x
- **Build command**: `npm run build`
- **Lint command**: `npm run lint`
- **Output directory**: `dist/`

### Environment Variables

The build process uses the `VITE_API_BASE_URL` environment variable to configure the API endpoint. This is set during the build step and embedded in the production bundle.

**Important**: The API base URL must be set before building, as Vite embeds environment variables at build time.

### Customizing the Pipeline

To customize the pipeline:

1. **Change Node.js version**:
   ```yaml
   variables:
     - name: nodeVersion
       value: '20.x'  # Change to desired version
   ```

2. **Enable/disable deployment options**:
   - Set `enabled: true` for the deployment method you want to use
   - Set `enabled: false` for methods you don't need
   - Update the `condition` to match your `deploymentTarget` variable

3. **Add additional build steps**:
   ```yaml
   - task: Npm@1
     displayName: 'Run tests'
     inputs:
       command: 'custom'
       workingDir: '$(frontendPath)'
       customCommand: 'run test'
   ```

---

## Deployment Options

The pipeline supports multiple deployment targets. Choose one based on your infrastructure:

### Option 1: Azure Static Web Apps

**Best for**: Modern static hosting with built-in CI/CD

**Setup**:
1. Create an Azure Static Web App in Azure Portal
2. Get the deployment token from Azure Portal
3. Set `AZURE_STATIC_WEB_APPS_API_TOKEN` in variable group
4. Set `deploymentTarget` to `AzureStaticWebApps`
5. Enable the `AzureStaticWebApp@0` task in the pipeline

**Advantages**:
- Free tier available
- Built-in CDN
- Automatic HTTPS
- Custom domains support

### Option 2: Azure Storage Static Website

**Best for**: Simple, cost-effective hosting

**Setup**:
1. Create an Azure Storage Account
2. Enable static website hosting
3. Set `storageAccountName` and `storageAccountKey` in variable group
4. Set `deploymentTarget` to `AzureStorage`
5. Enable the `AzureCLI@2` task in the pipeline

**Advantages**:
- Very low cost
- Scalable
- CDN integration available

### Option 3: IIS (Windows Server)

**Best for**: Windows-based hosting infrastructure

**Setup**:
1. Set up IIS on Windows Server
2. Configure service connection for the server
3. Set `deploymentTarget` to `IIS`
4. Enable the `IISWebAppManagementOnMachineGroup@0` task
5. Configure website name and bindings

**Advantages**:
- Full Windows integration
- .NET ecosystem compatibility

### Option 4: Linux Web Server (Nginx/Apache)

**Best for**: Linux-based hosting infrastructure

**Setup**:
1. Set up Nginx or Apache on Linux server
2. Create SSH service connection in Azure DevOps
3. Set `sshEndpoint` and `webServerPath` in variable group
4. Set `deploymentTarget` to `LinuxWebServer`
5. Enable the `SSH@0` task in the pipeline

**Advantages**:
- High performance
- Flexible configuration
- Cost-effective

### Option 5: File Copy (Generic)

**Best for**: Custom deployment scenarios

**Setup**:
1. Set `deploymentTargetPath` in variable group
2. Set `deploymentTarget` to `FileCopy`
3. Enable the `CopyFiles@2` task in the pipeline

**Advantages**:
- Maximum flexibility
- Works with any file system

---

## Release Process

### Manual Release

1. **Trigger the Pipeline**:
   - Go to **Pipelines** → **Pipelines**
   - Select your frontend release pipeline
   - Click **Run pipeline**
   - Select the branch (usually `main` or `master`)
   - Review variables and click **Run**

2. **Monitor Build Stage**:
   - Watch the build progress in real-time
   - Check for any errors or warnings
   - Verify artifacts are published

3. **Deploy to Staging**:
   - After build succeeds, staging deployment starts automatically
   - Review deployment logs
   - Test the staging environment

4. **Deploy to Production**:
   - After staging deployment succeeds, production deployment starts
   - If approvals are configured, wait for approval
   - Monitor production deployment
   - Verify production environment

### Automated Release

To trigger releases automatically:

1. **From Build Pipeline**:
   ```yaml
   - task: TriggerRelease@1
     inputs:
       definitionId: 'YOUR_RELEASE_PIPELINE_ID'
       projectId: 'YOUR_PROJECT_ID'
   ```

2. **From Git Push**:
   - Modify the pipeline YAML:
   ```yaml
   trigger:
     branches:
       include:
         - main
         - master
   ```

3. **Scheduled Releases**:
   - Add a schedule in pipeline settings
   - Go to pipeline → Edit → Triggers → Scheduled

### Pre-Release Checklist

Before releasing to production:

- [ ] All tests pass
- [ ] Code review completed
- [ ] API base URL is correct for production
- [ ] Environment variables are set correctly
- [ ] Deployment target is accessible
- [ ] Backup of current production version (if applicable)
- [ ] Rollback plan is ready
- [ ] Team is notified of release

### Post-Release Checklist

After releasing to production:

- [ ] Verify application loads correctly
- [ ] Test critical user flows
- [ ] Check API connectivity
- [ ] Monitor error logs
- [ ] Verify performance metrics
- [ ] Update release notes/documentation
- [ ] Notify stakeholders

---

## Troubleshooting

### Common Issues

#### Build Fails: Node.js Not Found
**Solution**: Ensure Node.js is installed on the build agent or use `NodeTool@0` task (already included in pipeline).

#### Build Fails: npm install Errors
**Solution**: 
- Check `package.json` for valid dependencies
- Clear npm cache: Add step to run `npm cache clean --force`
- Check network connectivity

#### Build Fails: TypeScript Errors
**Solution**:
- Fix TypeScript compilation errors locally first
- Run `npm run build` locally to verify
- Check `tsconfig.json` configuration

#### Deployment Fails: Authentication Errors
**Solution**:
- Verify service connections are configured correctly
- Check credentials in variable groups
- Ensure service principal has required permissions

#### Deployment Fails: File Not Found
**Solution**:
- Verify artifact is published correctly
- Check artifact path in download task
- Ensure build stage completed successfully

#### API Calls Fail After Deployment
**Solution**:
- Verify `VITE_API_BASE_URL` is set correctly
- Check API endpoint is accessible from deployment location
- Verify CORS settings on API server
- Check browser console for errors

#### Application Shows Blank Page
**Solution**:
- Check browser console for JavaScript errors
- Verify all files are deployed (check network tab)
- Ensure base path is configured correctly
- Check web server configuration (e.g., fallback to index.html for SPA)

### Debugging Tips

1. **Enable Verbose Logging**:
   - Add `system.debug: true` to pipeline variables
   - This shows detailed logs for troubleshooting

2. **Test Locally First**:
   ```bash
   cd SolutionDotnetReact/frontend
   npm install
   npm run build
   # Verify dist folder is created correctly
   ```

3. **Check Artifacts**:
   - Download artifacts from pipeline run
   - Verify files are present and correct

4. **Review Pipeline Logs**:
   - Check each task's logs for specific errors
   - Look for warnings that might indicate issues

5. **Validate Configuration**:
   - Double-check all variable values
   - Verify environment names match
   - Confirm deployment target settings

### Getting Help

If you encounter issues not covered here:

1. Check Azure DevOps documentation
2. Review pipeline logs in detail
3. Test deployment steps manually
4. Contact DevOps team or infrastructure support
5. Check application-specific logs on deployment target

---

## Best Practices

1. **Version Control**: Always commit pipeline changes to version control
2. **Testing**: Test pipeline changes in a separate branch first
3. **Security**: Never commit secrets to code; use variable groups
4. **Monitoring**: Set up alerts for failed deployments
5. **Documentation**: Keep this guide updated with environment-specific notes
6. **Backups**: Maintain backups of production deployments
7. **Rollback Plan**: Always have a rollback strategy ready
8. **Staging First**: Always deploy to staging before production
9. **Approvals**: Use approvals for production deployments
10. **Logging**: Keep detailed logs of all deployments

---

## Additional Resources

- [Azure DevOps Pipelines Documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/)
- [Vite Build Documentation](https://vitejs.dev/guide/build.html)
- [React Deployment Guide](https://react.dev/learn/start-a-new-react-project#deploying-your-app)
- [Azure Static Web Apps Documentation](https://docs.microsoft.com/en-us/azure/static-web-apps/)

---

## Support

For questions or issues related to the release process, contact:
- DevOps Team: [contact information]
- Development Team: [contact information]
- Infrastructure Team: [contact information]

