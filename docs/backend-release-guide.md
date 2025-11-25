# Backend Release Guide

This guide provides step-by-step instructions for setting up and managing releases of the UCConverter backend API using Azure DevOps release pipelines.

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

The backend release pipeline automates the build, testing, and deployment of the .NET 8 Web API application. The pipeline:
- Restores NuGet packages
- Builds the solution
- Runs unit tests with code coverage
- Publishes the API application
- Copies required configuration files (UnitsSettings)
- Deploys to staging and production environments

---

## Prerequisites

### Required Access
- Azure DevOps project with appropriate permissions
- Access to create and manage release pipelines
- Access to deployment targets (Azure, servers, etc.)

### Required Tools
- Azure DevOps account
- .NET 8 SDK (installed on build agents or via pipeline task)
- Access to deployment targets

### Required Information
- Deployment target credentials and configuration
- Environment-specific settings (connection strings, API keys, etc.)
- UnitsSettings folder location (if different from default)

---

## Pipeline Setup

### Step 1: Create a New Release Pipeline

1. Navigate to your Azure DevOps project
2. Go to **Pipelines** → **Pipelines**
3. Click **New pipeline**
4. Select your repository
5. Choose **Existing Azure Pipelines YAML file**
6. Select the branch and path: `SolutionDotnetReact/azure-pipelines-release-backend.yml`

### Step 2: Configure Pipeline Variables

Create a variable group named `BackendReleaseVariables`:

1. Go to **Pipelines** → **Library**
2. Click **+ Variable group**
3. Name it `BackendReleaseVariables`
4. Add the following variables:

| Variable Name | Description | Example | Secret |
|--------------|-------------|---------|--------|
| `deploymentTarget` | Target deployment method | `AzureAppService` | No |
| `azureSubscription` | Azure subscription name | `My Subscription` | No |
| `azureAppServiceName` | Azure App Service name (staging) | `ucconverter-api-staging` | No |
| `azureAppServiceNameProduction` | Azure App Service name (production) | `ucconverter-api-prod` | No |
| `resourceGroupName` | Azure resource group name | `ucconverter-rg` | No |
| `apiBaseUrl` | Staging API base URL | `https://api-staging.yourapp.com` | No |
| `apiBaseUrlProduction` | Production API base URL | `https://api.yourapp.com` | No |
| `webServerPath` | Linux web server path | `/var/www/ucconverter-api` | No |
| `deploymentTargetPath` | File copy target path | `\\server\share\api` | No |
| `dockerRegistryServiceConnection` | Docker registry service connection | `DockerHub` | No |
| `dockerImageRepository` | Docker image repository | `myorg/ucconverter-api` | No |
| `containerInstanceName` | Azure Container Instance name | `ucconverter-api-aci` | No |
| `dnsNameLabel` | DNS name label for ACI | `ucconverter-api` | No |
| `dockerRegistry` | Docker registry URL | `docker.io` | No |
| `dockerRegistryUsername` | Docker registry username | `myusername` | Yes |
| `dockerRegistryPassword` | Docker registry password | (password) | Yes |

**Note**: Mark sensitive values (passwords, tokens, keys) as **Secret** by checking the lock icon.

### Step 3: Create Service Connections

#### Azure Service Connection
1. Go to **Project Settings** → **Service connections**
2. Click **New service connection**
3. Select **Azure Resource Manager**
4. Choose authentication method (Service Principal recommended)
5. Select your Azure subscription
6. Name it (e.g., `AzureSubscription`)
7. Use this name in the `azureSubscription` variable

#### SSH Service Connection (for Linux deployment)
1. Go to **Project Settings** → **Service connections**
2. Click **New service connection**
3. Select **SSH**
4. Enter server details and credentials
5. Name it (e.g., `LinuxWebServer`)
6. Use this name in the `sshEndpoint` variable

#### Docker Registry Service Connection
1. Go to **Project Settings** → **Service connections**
2. Click **New service connection**
3. Select **Docker Registry**
4. Choose your registry (Docker Hub, Azure Container Registry, etc.)
5. Enter credentials
6. Name it (e.g., `DockerHub`)
7. Use this name in the `dockerRegistryServiceConnection` variable

### Step 4: Create Environments

1. Go to **Pipelines** → **Environments**
2. Click **Create environment**
3. Create two environments:
   - **Staging**: For testing before production
   - **Production**: For live deployment

4. For each environment, configure:
   - **Approvals**: Add required approvers (recommended for Production)
   - **Checks**: Add any required checks (e.g., security scans, smoke tests)
   - **Deployment targets**: Add your deployment targets

---

## Configuration

### Build Configuration

The pipeline uses the following default settings:
- **.NET SDK version**: 8.x
- **Build configuration**: Release
- **Solution file**: `UCConverter.sln`
- **API project**: `src/UCConverter.Api/UCConverter.Api.csproj`
- **Output directory**: `$(Build.ArtifactStagingDirectory)/publish`

### UnitsSettings Folder

The pipeline automatically copies the `UnitsSettings` folder to the publish output. This folder contains JSON configuration files for unit conversions and must be deployed with the application.

**Important**: Ensure the `UnitsSettings` folder is accessible from the deployment location. The application will look for it relative to the application base directory.

### Customizing the Pipeline

To customize the pipeline:

1. **Change .NET SDK version**:
   ```yaml
   - task: UseDotNet@2
     inputs:
       version: '7.x'  # Change to desired version
   ```

2. **Enable/disable deployment options**:
   - Set `enabled: true` for the deployment method you want to use
   - Set `enabled: false` for methods you don't need
   - Update the `condition` to match your `deploymentTarget` variable

3. **Add additional build steps**:
   ```yaml
   - task: DotNetCoreCLI@2
     displayName: 'Run code analysis'
     inputs:
       command: 'custom'
       custom: 'tool'
       arguments: 'run dotnet-format --check'
   ```

4. **Modify test coverage settings**:
   ```yaml
   - task: DotNetCoreCLI@2
     displayName: 'Run unit tests'
     inputs:
       arguments: '--configuration $(buildConfiguration) --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$(Agent.TempDirectory)/coverage/ /p:Threshold=95'
   ```

---

## Deployment Options

The pipeline supports multiple deployment targets. Choose one based on your infrastructure:

### Option 1: Azure App Service

**Best for**: Managed hosting with automatic scaling and deployment slots

**Setup**:
1. Create an Azure App Service in Azure Portal
2. Configure the app service (runtime stack: .NET 8)
3. Set `azureAppServiceName` in variable group
4. Set `deploymentTarget` to `AzureAppService`
5. Enable the `AzureWebApp@1` task in the pipeline

**Advantages**:
- Managed platform
- Automatic scaling
- Deployment slots for zero-downtime deployments
- Built-in monitoring and logging
- Easy SSL certificate management

**Configuration**:
- Set `ASPNETCORE_ENVIRONMENT` in App Service Configuration → Application Settings
- Configure connection strings if needed
- Set CORS origins for frontend

### Option 2: Azure App Service (Linux)

**Best for**: Linux-based hosting with .NET 8

**Setup**:
1. Create an Azure App Service with Linux runtime stack
2. Set runtime stack to `.NET 8`
3. Set `azureAppServiceName` in variable group
4. Set `deploymentTarget` to `AzureAppServiceLinux`
5. Enable the `AzureWebApp@1` task with Linux configuration

**Advantages**:
- Linux-based hosting
- Cost-effective
- Container support

### Option 3: IIS (Windows Server)

**Best for**: Windows-based hosting infrastructure

**Setup**:
1. Set up IIS on Windows Server
2. Install .NET 8 Hosting Bundle on the server
3. Configure service connection for the server
4. Set `deploymentTarget` to `IIS`
5. Enable both `IISWebAppManagementOnMachineGroup@0` and `IISWebAppDeploymentOnMachineGroup@0` tasks
6. Configure website name and bindings

**Advantages**:
- Full Windows integration
- .NET ecosystem compatibility
- Advanced IIS features

**Requirements**:
- .NET 8 Hosting Bundle installed on server
- IIS with ASP.NET Core Module
- Appropriate permissions for deployment

### Option 4: Linux Web Server (Nginx/Kestrel)

**Best for**: Linux-based hosting with custom configuration

**Setup**:
1. Set up Linux server with .NET 8 runtime
2. Configure Nginx as reverse proxy (optional)
3. Create systemd service for Kestrel
4. Create SSH service connection in Azure DevOps
5. Set `sshEndpoint` and `webServerPath` in variable group
6. Set `deploymentTarget` to `LinuxWebServer`
7. Enable the `SSH@0` task in the pipeline

**Advantages**:
- High performance
- Flexible configuration
- Cost-effective
- Full control

**Systemd Service Example**:
```ini
[Unit]
Description=UCConverter API
After=network.target

[Service]
Type=notify
ExecStart=/usr/bin/dotnet /var/www/ucconverter-api/UCConverter.Api.dll
Restart=always
RestartSec=10
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

### Option 5: Docker

**Best for**: Containerized deployments

**Setup**:
1. Create a `Dockerfile` in the solution root:
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 80
   EXPOSE 443

   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src
   COPY ["src/UCConverter.Api/UCConverter.Api.csproj", "src/UCConverter.Api/"]
   COPY ["src/UCConverter.Application/UCConverter.Application.csproj", "src/UCConverter.Application/"]
   COPY ["src/UCConverter.Domain/UCConverter.Domain.csproj", "src/UCConverter.Domain/"]
   COPY ["src/UCConverter.Infrastructure/UCConverter.Infrastructure.csproj", "src/UCConverter.Infrastructure/"]
   COPY ["UnitsSettings/", "UnitsSettings/"]
   RUN dotnet restore "src/UCConverter.Api/UCConverter.Api.csproj"
   COPY . .
   WORKDIR "/src/src/UCConverter.Api"
   RUN dotnet build "UCConverter.Api.csproj" -c Release -o /app/build

   FROM build AS publish
   RUN dotnet publish "UCConverter.Api.csproj" -c Release -o /app/publish

   FROM base AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   COPY --from=publish /app/UnitsSettings ./UnitsSettings
   ENTRYPOINT ["dotnet", "UCConverter.Api.dll"]
   ```

2. Create Docker registry service connection
3. Set `dockerRegistryServiceConnection` and `dockerImageRepository` in variable group
4. Set `deploymentTarget` to `Docker`
5. Enable the `Docker@2` task in the pipeline

**Advantages**:
- Consistent environments
- Easy scaling
- Platform agnostic
- Version control for deployments

### Option 6: Azure Container Instances

**Best for**: Simple container hosting without orchestration

**Setup**:
1. Follow Docker setup above
2. Set `resourceGroupName`, `containerInstanceName`, and `dnsNameLabel` in variable group
3. Set `deploymentTarget` to `AzureContainerInstances`
4. Enable the `AzureCLI@2` task for ACI deployment

**Advantages**:
- Simple container hosting
- Pay-per-use
- Quick deployment
- No orchestration overhead

### Option 7: File Copy (Generic)

**Best for**: Custom deployment scenarios

**Setup**:
1. Set `deploymentTargetPath` in variable group
2. Set `deploymentTarget` to `FileCopy`
3. Enable the `CopyFiles@2` task in the pipeline

**Advantages**:
- Maximum flexibility
- Works with any file system
- Custom deployment scripts possible

---

## Release Process

### Manual Release

1. **Trigger the Pipeline**:
   - Go to **Pipelines** → **Pipelines**
   - Select your backend release pipeline
   - Click **Run pipeline**
   - Select the branch (usually `main` or `master`)
   - Review variables and click **Run**

2. **Monitor Build Stage**:
   - Watch the build progress in real-time
   - Check for any compilation errors
   - Verify tests pass
   - Check code coverage results
   - Verify artifacts are published

3. **Deploy to Staging**:
   - After build succeeds, staging deployment starts automatically
   - Review deployment logs
   - Verify health check passes
   - Test the staging API endpoints
   - Check Swagger UI: `https://your-staging-api/swagger`

4. **Deploy to Production**:
   - After staging deployment succeeds, production deployment starts
   - If approvals are configured, wait for approval
   - Monitor production deployment
   - Verify production environment
   - Check Swagger UI: `https://your-production-api/swagger`

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
- [ ] Code coverage meets requirements (≥95%)
- [ ] Code review completed
- [ ] Environment variables are set correctly
- [ ] Connection strings are configured
- [ ] UnitsSettings folder is up to date
- [ ] CORS settings are configured for frontend
- [ ] Deployment target is accessible
- [ ] Backup of current production version (if applicable)
- [ ] Rollback plan is ready
- [ ] Team is notified of release
- [ ] Database migrations are ready (if applicable)

### Post-Release Checklist

After releasing to production:

- [ ] Verify API is responding (health check)
- [ ] Test critical API endpoints
- [ ] Verify Swagger UI is accessible
- [ ] Check application logs for errors
- [ ] Verify UnitsSettings are loaded correctly
- [ ] Test unit conversion functionality
- [ ] Monitor performance metrics
- [ ] Verify frontend can connect to API
- [ ] Check CORS is working correctly
- [ ] Update release notes/documentation
- [ ] Notify stakeholders

---

## Troubleshooting

### Common Issues

#### Build Fails: .NET SDK Not Found
**Solution**: Ensure .NET 8 SDK is installed on the build agent or use `UseDotNet@2` task (already included in pipeline).

#### Build Fails: NuGet Restore Errors
**Solution**: 
- Check network connectivity
- Verify NuGet package sources are accessible
- Check for package version conflicts
- Clear NuGet cache if needed

#### Build Fails: Compilation Errors
**Solution**:
- Fix compilation errors locally first
- Run `dotnet build` locally to verify
- Check for missing project references

#### Tests Fail
**Solution**:
- Review test output for specific failures
- Run tests locally: `dotnet test`
- Check for environment-specific issues
- Verify test data and mocks

#### Deployment Fails: Authentication Errors
**Solution**:
- Verify service connections are configured correctly
- Check credentials in variable groups
- Ensure service principal has required permissions
- Verify Azure subscription access

#### Deployment Fails: UnitsSettings Not Found
**Solution**:
- Verify UnitsSettings folder is copied in build stage
- Check folder path in deployment location
- Ensure application has read permissions
- Verify relative path configuration in Program.cs

#### API Not Responding After Deployment
**Solution**:
- Check application logs
- Verify .NET runtime is installed on server
- Check firewall and network settings
- Verify application is running (check process/service status)
- Review application configuration (appsettings.json)

#### CORS Errors
**Solution**:
- Verify CORS configuration in Program.cs
- Check allowed origins match frontend URL
- Verify CORS middleware is in correct order
- Check browser console for specific CORS errors

#### Swagger Not Accessible
**Solution**:
- Verify Swagger is enabled in Program.cs
- Check route configuration
- Verify environment settings (Swagger may be disabled in Production)
- Check URL path: `/swagger`

### Debugging Tips

1. **Enable Verbose Logging**:
   - Add `system.debug: true` to pipeline variables
   - This shows detailed logs for troubleshooting

2. **Test Locally First**:
   ```bash
   cd SolutionDotnetReact
   dotnet restore
   dotnet build
   dotnet test
   dotnet publish src/UCConverter.Api/UCConverter.Api.csproj -c Release
   ```

3. **Check Artifacts**:
   - Download artifacts from pipeline run
   - Verify all files are present (including UnitsSettings)
   - Check file structure matches expectations

4. **Review Pipeline Logs**:
   - Check each task's logs for specific errors
   - Look for warnings that might indicate issues
   - Check deployment logs on target server

5. **Validate Configuration**:
   - Double-check all variable values
   - Verify environment names match
   - Confirm deployment target settings
   - Check appsettings.json configuration

6. **Check Application Logs**:
   - Azure App Service: Use Log Stream in Azure Portal
   - IIS: Check Event Viewer and application logs
   - Linux: Check systemd journal: `journalctl -u ucconverter-api`

### Getting Help

If you encounter issues not covered here:

1. Check Azure DevOps documentation
2. Review pipeline logs in detail
3. Test deployment steps manually
4. Check application logs on deployment target
5. Contact DevOps team or infrastructure support
6. Review .NET 8 deployment documentation

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
11. **Health Checks**: Implement and monitor health check endpoints
12. **Code Coverage**: Maintain high test coverage (≥95%)
13. **Configuration Management**: Use environment-specific configuration
14. **Zero-Downtime**: Use deployment slots or blue-green deployments for production

---

## Additional Resources

- [Azure DevOps Pipelines Documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/)
- [.NET 8 Deployment Documentation](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/)
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [IIS Deployment Guide](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/)
- [Docker Documentation](https://docs.docker.com/)

---

## Support

For questions or issues related to the release process, contact:
- DevOps Team: [contact information]
- Development Team: [contact information]
- Infrastructure Team: [contact information]

