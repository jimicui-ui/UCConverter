# Phase 4 Requirements - Extended Unit Categories: Pressure, Energy, Power, Electricity, Engineering, and Heat Converters

## Table of Contents
1. [Overview](#1-overview)
2. [New Unit Categories](#2-new-unit-categories)
   - [2.1 Pressure Converter](#21-pressure-converter)
   - [2.2 Energy Converter](#22-energy-converter)
   - [2.3 Power Converter](#23-power-converter)
   - [2.4 Electricity Converters](#24-electricity-converters)
   - [2.5 Engineering Converters](#25-engineering-converters)
   - [2.6 Heat Converters](#26-heat-converters)
3. [Implementation Requirements](#3-implementation-requirements)
   - [3.1 JSON Configuration Files](#31-json-configuration-files)
   - [3.2 Backend Implementation](#32-backend-implementation)
   - [3.3 Frontend Implementation](#33-frontend-implementation)
   - [3.4 Localization](#34-localization)
4. [Unit Definitions](#4-unit-definitions)
   - [4.1 Pressure Units](#41-pressure-units)
   - [4.2 Energy Units](#42-energy-units)
   - [4.3 Power Units](#43-power-units)
   - [4.4 Electricity Units](#44-electricity-units)
   - [4.5 Engineering Units](#45-engineering-units)
   - [4.6 Heat Units](#46-heat-units)
5. [Implementation Priorities](#5-implementation-priorities)
6. [Success Criteria](#6-success-criteria)
7. [Testing Requirements](#7-testing-requirements)

---

## 1. Overview

Phase 4 focuses on adding comprehensive unit conversion support for **Pressure**, **Energy**, **Power**, **Electricity Converters**, **Engineering Converters**, and **Heat Converters**. This phase significantly expands the application's capabilities to cover essential physical quantities used in engineering, physics, and everyday applications.

**New Feature: Category Grouping**

Phase 4 also introduces a **category grouping feature** that organizes converters into logical groups:
- **Common**: Basic everyday converters (length, weight, volume, temperature, area, time, speed, pressure, energy, power)
- **Engineering**: Engineering and mechanical converters (angular velocity, acceleration, density, torque, etc.)
- **Electricity**: Electrical and electromagnetic converters (charge, current, voltage, resistance, capacitance, etc.)
- **Heat**: Thermal and heat-related converters (thermal conductivity, specific heat capacity, fuel efficiency, etc.)

This grouping feature improves user experience by making it easier to find specific converter types through radio button selection and filtered category lists.

### Scope

This phase includes:

- **Pressure Converter**: Support for various pressure units (pascal, bar, psi, atmosphere, etc.)
- **Energy Converter**: Support for energy/work units (joule, calorie, kilowatt-hour, BTU, etc.)
- **Power Converter**: Support for power units (watt, horsepower, BTU/hour, etc.)
- **Electricity Converters**: Comprehensive set of electrical and electromagnetic unit converters:
  - Charge Converter
  - Linear Charge Density Converter
  - Surface Charge Density Converter
  - Volume Charge Density Converter
  - Current Converter
  - Linear Current Density Converter
  - Surface Current Density Converter
  - Electric Field Strength Converter
  - Electric Potential Converter
  - Electric Resistance Converter
  - Electric Resistivity Converter
  - Electric Conductance Converter
  - Electric Conductivity Converter
  - Electrostatic Capacitance Converter
  - Inductance Converter
- **Engineering Converters**: Essential engineering and mechanical unit converters:
  - Velocity - Angular Converter
  - Acceleration Converter
  - Acceleration - Angular Converter
  - Density Converter
  - Specific Volume Converter
  - Moment of Inertia Converter
  - Moment of Force Converter
  - Torque Converter
- **Heat Converters**: Comprehensive thermal and heat-related unit converters:
  - Fuel Efficiency - Mass Converter
  - Fuel Efficiency - Volume Converter
  - Temperature Interval Converter
  - Thermal Expansion Converter
  - Thermal Resistance Converter
  - Thermal Conductivity Converter
  - Specific Heat Capacity Converter
  - Heat Density Converter
  - Heat Flux Density Converter
  - Heat Transfer Coefficient Converter

### Goals

- Extend the unit converter application with essential physical quantities
- Maintain consistency with existing SI-based conversion system
- Ensure all new categories follow the same architecture and patterns as existing categories
- Provide comprehensive localization support for all new categories and units
- Implement category grouping feature to organize converters by type (Common, Engineering, Electricity, Heat)
- Maintain high code quality with ≥95% test coverage

---

## 2. New Unit Categories

### 2.1 Pressure Converter

**Category Name**: `pressure`

**Base Unit (SI Derived Unit)**: pascal (Pa) = N/m² = kg/(m·s²) ⭐

**Description**: Pressure is the force applied perpendicular to the surface of an object per unit area. The SI derived unit for pressure is the pascal (Pa), which is equivalent to one newton per square meter.

**Common Use Cases**:
- Atmospheric pressure measurements
- Tire pressure (PSI)
- Barometric pressure
- Engineering pressure calculations
- Hydraulic systems

**Unit Systems**:
- **SI**: pascal (Pa), kilopascal (kPa), megapascal (MPa)
- **Non-SI Metric**: bar, millibar (mbar)
- **Imperial/US Customary**: pound per square inch (psi), pound per square foot (psf)
- **Other**: atmosphere (atm), torr, millimeter of mercury (mmHg)

### 2.2 Energy Converter

**Category Name**: `energy`

**Base Unit (SI Derived Unit)**: joule (J) = N·m = kg·m²/s² ⭐

**Description**: Energy is the capacity to do work. The SI derived unit for energy is the joule (J), which is equivalent to one newton-meter or one watt-second.

**Common Use Cases**:
- Food energy (calories)
- Electrical energy (kilowatt-hours)
- Thermal energy (BTU)
- Mechanical work
- Chemical energy

**Unit Systems**:
- **SI**: joule (J), kilojoule (kJ), megajoule (MJ)
- **Non-SI Metric**: calorie (cal), kilocalorie (kcal)
- **Imperial/US Customary**: British Thermal Unit (BTU), foot-pound (ft·lb)
- **Other**: electronvolt (eV), therm, quad

### 2.3 Power Converter

**Category Name**: `power`

**Base Unit (SI Derived Unit)**: watt (W) = J/s = kg·m²/s³ ⭐

**Description**: Power is the rate at which work is done or energy is transferred. The SI derived unit for power is the watt (W), which is equivalent to one joule per second.

**Common Use Cases**:
- Electrical power ratings
- Engine horsepower
- Heating/cooling capacity
- Solar panel output
- Appliance power consumption

**Unit Systems**:
- **SI**: watt (W), kilowatt (kW), megawatt (MW), gigawatt (GW)
- **Imperial/US Customary**: horsepower (hp), foot-pound per second (ft·lb/s), BTU per hour (BTU/h)
- **Other**: erg per second (erg/s)

### 2.4 Electricity Converters

This section covers all electrical and electromagnetic unit converters to be implemented in Phase 4.

#### 2.4.1 Charge Converter

**Category Name**: `charge`

**Base Unit (SI Base Unit)**: coulomb (C) = A·s ⭐

**Description**: Electric charge is a fundamental property of matter. The SI derived unit for charge is the coulomb (C), which is equivalent to one ampere-second.

**Common Use Cases**:
- Capacitor charge
- Battery capacity
- Electrostatic charge
- Current integration

**Unit Systems**:
- **SI**: coulomb (C), millicoulomb (mC), microcoulomb (µC)
- **CGS**: statcoulomb (statC), franklin (Fr)
- **Other**: ampere-hour (A·h), milliampere-hour (mA·h)

#### 2.4.2 Linear Charge Density Converter

**Category Name**: `linearChargeDensity`

**Base Unit (SI Derived Unit)**: coulomb per meter (C/m) ⭐

**Description**: Linear charge density is the amount of electric charge per unit length. The SI derived unit is coulomb per meter (C/m).

**Common Use Cases**:
- Charge distribution along a line
- Wire charge density
- Theoretical physics calculations

**Unit Systems**:
- **SI**: coulomb per meter (C/m), coulomb per centimeter (C/cm)
- **CGS**: statcoulomb per centimeter (statC/cm)

#### 2.4.3 Surface Charge Density Converter

**Category Name**: `surfaceChargeDensity`

**Base Unit (SI Derived Unit)**: coulomb per square meter (C/m²) ⭐

**Description**: Surface charge density is the amount of electric charge per unit area. The SI derived unit is coulomb per square meter (C/m²).

**Common Use Cases**:
- Charge distribution on surfaces
- Capacitor plate charge density
- Electrostatic field calculations

**Unit Systems**:
- **SI**: coulomb per square meter (C/m²), coulomb per square centimeter (C/cm²)
- **CGS**: statcoulomb per square centimeter (statC/cm²)

#### 2.4.4 Volume Charge Density Converter

**Category Name**: `volumeChargeDensity`

**Base Unit (SI Derived Unit)**: coulomb per cubic meter (C/m³) ⭐

**Description**: Volume charge density is the amount of electric charge per unit volume. The SI derived unit is coulomb per cubic meter (C/m³).

**Common Use Cases**:
- Charge distribution in volumes
- Plasma physics
- Semiconductor charge density

**Unit Systems**:
- **SI**: coulomb per cubic meter (C/m³), coulomb per cubic centimeter (C/cm³)
- **CGS**: statcoulomb per cubic centimeter (statC/cm³)

#### 2.4.5 Current Converter

**Category Name**: `current`

**Base Unit (SI Base Unit)**: ampere (A) ⭐

**Description**: Electric current is the flow of electric charge. The SI base unit for current is the ampere (A).

**Common Use Cases**:
- Electrical circuit current
- Battery current
- Power supply ratings
- Fuse ratings

**Unit Systems**:
- **SI**: ampere (A), milliampere (mA), microampere (µA), kiloampere (kA)
- **CGS**: statampere (statA), abampere (abA)
- **Other**: biot (Bi)

#### 2.4.6 Linear Current Density Converter

**Category Name**: `linearCurrentDensity`

**Base Unit (SI Derived Unit)**: ampere per meter (A/m) ⭐

**Description**: Linear current density is the amount of electric current per unit length. The SI derived unit is ampere per meter (A/m).

**Common Use Cases**:
- Current distribution along conductors
- Solenoid calculations
- Electromagnetic field calculations

**Unit Systems**:
- **SI**: ampere per meter (A/m), ampere per centimeter (A/cm)
- **CGS**: abampere per centimeter (abA/cm)

#### 2.4.7 Surface Current Density Converter

**Category Name**: `surfaceCurrentDensity`

**Base Unit (SI Derived Unit)**: ampere per meter (A/m) ⭐

**Description**: Surface current density is the amount of electric current per unit width flowing along a surface. The SI derived unit is ampere per meter (A/m).

**Common Use Cases**:
- Current sheet calculations
- Electromagnetic boundary conditions
- Theoretical physics

**Unit Systems**:
- **SI**: ampere per meter (A/m), ampere per centimeter (A/cm)
- **CGS**: abampere per centimeter (abA/cm)

#### 2.4.8 Electric Field Strength Converter

**Category Name**: `electricFieldStrength`

**Base Unit (SI Derived Unit)**: volt per meter (V/m) = N/C = kg·m/(A·s³) ⭐

**Description**: Electric field strength is the force per unit charge experienced by a test charge. The SI derived unit is volt per meter (V/m).

**Common Use Cases**:
- Electrostatic field calculations
- Capacitor field strength
- Lightning field strength
- Electrical safety standards

**Unit Systems**:
- **SI**: volt per meter (V/m), kilovolt per meter (kV/m), volt per centimeter (V/cm)
- **CGS**: statvolt per centimeter (statV/cm)

#### 2.4.9 Electric Potential Converter

**Category Name**: `electricPotential`

**Base Unit (SI Derived Unit)**: volt (V) = J/C = kg·m²/(A·s³) ⭐

**Description**: Electric potential (voltage) is the electric potential energy per unit charge. The SI derived unit is the volt (V).

**Common Use Cases**:
- Battery voltage
- Power supply voltage
- Circuit voltage measurements
- Electrical safety

**Unit Systems**:
- **SI**: volt (V), millivolt (mV), kilovolt (kV), megavolt (MV)
- **CGS**: statvolt (statV)
- **Other**: abvolt (abV)

#### 2.4.10 Electric Resistance Converter

**Category Name**: `electricResistance`

**Base Unit (SI Derived Unit)**: ohm (Ω) = V/A = kg·m²/(A²·s³) ⭐

**Description**: Electric resistance is the opposition to the flow of electric current. The SI derived unit is the ohm (Ω).

**Common Use Cases**:
- Resistor values
- Circuit resistance calculations
- Electrical component specifications
- Wire resistance

**Unit Systems**:
- **SI**: ohm (Ω), milliohm (mΩ), kiloohm (kΩ), megaohm (MΩ), gigaohm (GΩ)
- **CGS**: statohm, abohm

#### 2.4.11 Electric Resistivity Converter

**Category Name**: `electricResistivity`

**Base Unit (SI Derived Unit)**: ohm meter (Ω·m) ⭐

**Description**: Electric resistivity is a measure of how strongly a material opposes the flow of electric current. The SI derived unit is ohm meter (Ω·m).

**Common Use Cases**:
- Material property specifications
- Wire material selection
- Semiconductor properties
- Conductor design

**Unit Systems**:
- **SI**: ohm meter (Ω·m), ohm centimeter (Ω·cm), microohm meter (µΩ·m)
- **CGS**: ohm centimeter (Ω·cm), abohm centimeter (abΩ·cm)

#### 2.4.12 Electric Conductance Converter

**Category Name**: `electricConductance`

**Base Unit (SI Derived Unit)**: siemens (S) = 1/Ω = A²·s³/(kg·m²) ⭐

**Description**: Electric conductance is the reciprocal of resistance, measuring how easily electric current flows. The SI derived unit is the siemens (S), formerly called mho.

**Common Use Cases**:
- Conductance measurements
- Circuit analysis
- Material conductivity (inverse of resistivity)

**Unit Systems**:
- **SI**: siemens (S), millisiemens (mS), microsiemens (µS), kilosiemens (kS)
- **Other**: mho (℧) - obsolete unit, equivalent to siemens

#### 2.4.13 Electric Conductivity Converter

**Category Name**: `electricConductivity`

**Base Unit (SI Derived Unit)**: siemens per meter (S/m) ⭐

**Description**: Electric conductivity is the reciprocal of resistivity, measuring how well a material conducts electric current. The SI derived unit is siemens per meter (S/m).

**Common Use Cases**:
- Material property specifications
- Water conductivity (TDS measurements)
- Semiconductor properties
- Conductor material selection

**Unit Systems**:
- **SI**: siemens per meter (S/m), siemens per centimeter (S/cm), millisiemens per meter (mS/m)
- **Other**: mho per meter (℧/m) - obsolete unit

#### 2.4.14 Electrostatic Capacitance Converter

**Category Name**: `capacitance`

**Base Unit (SI Derived Unit)**: farad (F) = C/V = A²·s⁴/(kg·m²) ⭐

**Description**: Capacitance is the ability of a system to store electric charge. The SI derived unit is the farad (F).

**Common Use Cases**:
- Capacitor values
- Circuit design
- Energy storage systems
- Filter design

**Unit Systems**:
- **SI**: farad (F), millifarad (mF), microfarad (µF), nanofarad (nF), picofarad (pF)
- **CGS**: statfarad (statF), abfarad (abF)

#### 2.4.15 Inductance Converter

**Category Name**: `inductance`

**Base Unit (SI Derived Unit)**: henry (H) = V·s/A = kg·m²/(A²·s²) ⭐

**Description**: Inductance is the property of an electrical conductor that opposes changes in electric current. The SI derived unit is the henry (H).

**Common Use Cases**:
- Inductor values
- Transformer design
- Circuit analysis
- Filter design

**Unit Systems**:
- **SI**: henry (H), millihenry (mH), microhenry (µH), nanohenry (nH)
- **CGS**: abhenry (abH), stathenry (statH)

### 2.5 Engineering Converters

This section covers essential engineering and mechanical unit converters to be implemented in Phase 4.

#### 2.5.1 Velocity - Angular Converter

**Category Name**: `angularVelocity`

**Base Unit (SI Derived Unit)**: radian per second (rad/s) ⭐

**Description**: Angular velocity is the rate of change of angular displacement. The SI derived unit is radian per second (rad/s).

**Common Use Cases**:
- Rotational speed measurements
- Motor RPM conversions
- Gyroscope measurements
- Mechanical engineering calculations

**Unit Systems**:
- **SI**: radian per second (rad/s), radian per minute (rad/min)
- **Non-SI**: degree per second (°/s), degree per minute (°/min), revolution per second (rps), revolution per minute (rpm)

#### 2.5.2 Acceleration Converter

**Category Name**: `acceleration`

**Base Unit (SI Derived Unit)**: meter per second squared (m/s²) ⭐

**Description**: Acceleration is the rate of change of velocity. The SI derived unit is meter per second squared (m/s²).

**Common Use Cases**:
- Physics calculations
- Vehicle acceleration
- Free fall calculations
- Engineering dynamics

**Unit Systems**:
- **SI**: meter per second squared (m/s²), kilometer per hour squared (km/h²)
- **Imperial/US Customary**: foot per second squared (ft/s²), inch per second squared (in/s²)
- **Other**: standard gravity (g) = 9.80665 m/s², gal (Gal) = 0.01 m/s²

#### 2.5.3 Acceleration - Angular Converter

**Category Name**: `angularAcceleration`

**Base Unit (SI Derived Unit)**: radian per second squared (rad/s²) ⭐

**Description**: Angular acceleration is the rate of change of angular velocity. The SI derived unit is radian per second squared (rad/s²).

**Common Use Cases**:
- Rotational dynamics
- Motor control
- Mechanical engineering
- Robotics

**Unit Systems**:
- **SI**: radian per second squared (rad/s²)
- **Non-SI**: degree per second squared (°/s²), revolution per second squared (rps²), revolution per minute squared (rpm²)

#### 2.5.4 Density Converter

**Category Name**: `density`

**Base Unit (SI Derived Unit)**: kilogram per cubic meter (kg/m³) ⭐

**Description**: Density is mass per unit volume. The SI derived unit is kilogram per cubic meter (kg/m³).

**Common Use Cases**:
- Material properties
- Fluid mechanics
- Buoyancy calculations
- Engineering design

**Unit Systems**:
- **SI**: kilogram per cubic meter (kg/m³), gram per cubic centimeter (g/cm³), gram per liter (g/L)
- **Imperial/US Customary**: pound per cubic foot (lb/ft³), pound per cubic inch (lb/in³), ounce per cubic inch (oz/in³)
- **Other**: slug per cubic foot (slug/ft³)

#### 2.5.5 Specific Volume Converter

**Category Name**: `specificVolume`

**Base Unit (SI Derived Unit)**: cubic meter per kilogram (m³/kg) ⭐

**Description**: Specific volume is the volume per unit mass (reciprocal of density). The SI derived unit is cubic meter per kilogram (m³/kg).

**Common Use Cases**:
- Thermodynamics
- Fluid properties
- Material science
- Engineering calculations

**Unit Systems**:
- **SI**: cubic meter per kilogram (m³/kg), liter per kilogram (L/kg), cubic centimeter per gram (cm³/g)
- **Imperial/US Customary**: cubic foot per pound (ft³/lb), cubic inch per pound (in³/lb), gallon per pound (gal/lb)

#### 2.5.6 Moment of Inertia Converter

**Category Name**: `momentOfInertia`

**Base Unit (SI Derived Unit)**: kilogram square meter (kg·m²) ⭐

**Description**: Moment of inertia is a measure of an object's resistance to rotational motion. The SI derived unit is kilogram square meter (kg·m²).

**Common Use Cases**:
- Rotational dynamics
- Mechanical engineering
- Physics calculations
- Structural analysis

**Unit Systems**:
- **SI**: kilogram square meter (kg·m²), gram square centimeter (g·cm²)
- **Imperial/US Customary**: pound square foot (lb·ft²), pound square inch (lb·in²), slug square foot (slug·ft²)
- **Other**: ounce square inch (oz·in²)

#### 2.5.7 Moment of Force Converter

**Category Name**: `momentOfForce`

**Base Unit (SI Derived Unit)**: newton meter (N·m) = kg·m²/s² ⭐

**Description**: Moment of force (torque) is the rotational equivalent of force. The SI derived unit is newton meter (N·m).

**Common Use Cases**:
- Torque measurements
- Mechanical engineering
- Bolt tightening specifications
- Motor specifications

**Unit Systems**:
- **SI**: newton meter (N·m), kilonewton meter (kN·m), newton centimeter (N·cm)
- **Imperial/US Customary**: pound-force foot (lbf·ft), pound-force inch (lbf·in), ounce-force inch (ozf·in)
- **Other**: kilogram-force meter (kgf·m), dyne centimeter (dyn·cm)

#### 2.5.8 Torque Converter

**Category Name**: `torque`

**Base Unit (SI Derived Unit)**: newton meter (N·m) = kg·m²/s² ⭐

**Description**: Torque is a measure of the force that can cause an object to rotate. The SI derived unit is newton meter (N·m), which is equivalent to moment of force.

**Common Use Cases**:
- Engine torque specifications
- Mechanical engineering
- Automotive applications
- Industrial machinery

**Unit Systems**:
- **SI**: newton meter (N·m), kilonewton meter (kN·m), newton centimeter (N·cm)
- **Imperial/US Customary**: pound-force foot (lbf·ft), pound-force inch (lbf·in), ounce-force inch (ozf·in)
- **Other**: kilogram-force meter (kgf·m), dyne centimeter (dyn·cm)

**Note**: Torque and Moment of Force are physically the same quantity, but may be implemented as separate categories for user convenience or if different unit sets are preferred.

### 2.6 Heat Converters

This section covers comprehensive thermal and heat-related unit converters to be implemented in Phase 4.

#### 2.6.1 Fuel Efficiency - Mass Converter

**Category Name**: `fuelEfficiencyMass`

**Base Unit (SI Derived Unit)**: meter per kilogram (m/kg) ⭐

**Description**: Fuel efficiency (mass-based) measures distance traveled per unit mass of fuel. The SI derived unit is meter per kilogram (m/kg).

**Common Use Cases**:
- Vehicle fuel economy
- Transportation efficiency
- Energy consumption analysis
- Environmental impact

**Unit Systems**:
- **SI**: meter per kilogram (m/kg), kilometer per kilogram (km/kg)
- **Imperial/US Customary**: mile per pound (mi/lb), foot per pound (ft/lb)
- **Other**: nautical mile per pound (nmi/lb)

#### 2.6.2 Fuel Efficiency - Volume Converter

**Category Name**: `fuelEfficiencyVolume`

**Base Unit (SI Derived Unit)**: meter per cubic meter (m/m³) ⭐

**Description**: Fuel efficiency (volume-based) measures distance traveled per unit volume of fuel. The SI derived unit is meter per cubic meter (m/m³), commonly expressed as liters per 100 kilometers (L/100km) or miles per gallon (mpg).

**Common Use Cases**:
- Vehicle fuel economy (mpg, L/100km)
- Transportation efficiency
- Fuel consumption analysis
- Automotive specifications

**Unit Systems**:
- **SI**: meter per cubic meter (m/m³), kilometer per liter (km/L), liter per 100 kilometers (L/100km)
- **Imperial/US Customary**: mile per gallon (mpg), mile per gallon (US) (mpg US), mile per gallon (UK) (mpg UK)
- **Other**: nautical mile per gallon (nmi/gal)

#### 2.6.3 Temperature Interval Converter

**Category Name**: `temperatureInterval`

**Base Unit (SI Base Unit)**: kelvin (K) ⭐

**Description**: Temperature interval is the difference between two temperatures. The SI base unit is kelvin (K), which is the same as the temperature unit but used for intervals.

**Common Use Cases**:
- Temperature difference calculations
- Thermal analysis
- Heat transfer calculations
- Engineering specifications

**Unit Systems**:
- **SI**: kelvin (K), degree Celsius (°C) - for intervals
- **Non-SI**: degree Fahrenheit (°F) - for intervals, degree Rankine (°R) - for intervals

**Note**: Temperature intervals use the same units as temperature but represent differences rather than absolute values.

#### 2.6.4 Thermal Expansion Converter

**Category Name**: `thermalExpansion`

**Base Unit (SI Derived Unit)**: per kelvin (1/K) or per degree Celsius (1/°C) ⭐

**Description**: Thermal expansion coefficient measures how much a material expands per degree of temperature change. The SI derived unit is per kelvin (1/K).

**Common Use Cases**:
- Material properties
- Engineering design
- Construction calculations
- Thermal stress analysis

**Unit Systems**:
- **SI**: per kelvin (1/K), per degree Celsius (1/°C)
- **Non-SI**: per degree Fahrenheit (1/°F), per degree Rankine (1/°R)

#### 2.6.5 Thermal Resistance Converter

**Category Name**: `thermalResistance`

**Base Unit (SI Derived Unit)**: kelvin per watt (K/W) = m²·K·s³/(kg·m²) ⭐

**Description**: Thermal resistance measures the resistance to heat flow. The SI derived unit is kelvin per watt (K/W).

**Common Use Cases**:
- Heat transfer calculations
- Insulation specifications
- Thermal management
- HVAC design

**Unit Systems**:
- **SI**: kelvin per watt (K/W), degree Celsius per watt (°C/W)
- **Imperial/US Customary**: hour square foot degree Fahrenheit per BTU (h·ft²·°F/BTU)
- **Other**: square meter kelvin per watt (m²·K/W) - for area-specific thermal resistance

#### 2.6.6 Thermal Conductivity Converter

**Category Name**: `thermalConductivity`

**Base Unit (SI Derived Unit)**: watt per meter kelvin (W/(m·K)) = kg·m/(s³·K) ⭐

**Description**: Thermal conductivity measures a material's ability to conduct heat. The SI derived unit is watt per meter kelvin (W/(m·K)).

**Common Use Cases**:
- Material properties
- Heat transfer calculations
- Insulation design
- Engineering specifications

**Unit Systems**:
- **SI**: watt per meter kelvin (W/(m·K)), watt per centimeter kelvin (W/(cm·K))
- **Imperial/US Customary**: BTU per hour foot degree Fahrenheit (BTU/(h·ft·°F)), calorie per second centimeter degree Celsius (cal/(s·cm·°C))
- **Other**: kilocalorie per hour meter degree Celsius (kcal/(h·m·°C))

#### 2.6.7 Specific Heat Capacity Converter

**Category Name**: `specificHeatCapacity`

**Base Unit (SI Derived Unit)**: joule per kilogram kelvin (J/(kg·K)) = m²/(s²·K) ⭐

**Description**: Specific heat capacity is the amount of heat required to raise the temperature of a unit mass by one degree. The SI derived unit is joule per kilogram kelvin (J/(kg·K)).

**Common Use Cases**:
- Thermodynamics
- Material properties
- Heat transfer calculations
- Engineering design

**Unit Systems**:
- **SI**: joule per kilogram kelvin (J/(kg·K)), kilojoule per kilogram kelvin (kJ/(kg·K))
- **Imperial/US Customary**: BTU per pound degree Fahrenheit (BTU/(lb·°F)), calorie per gram degree Celsius (cal/(g·°C))
- **Other**: kilocalorie per kilogram degree Celsius (kcal/(kg·°C))

#### 2.6.8 Heat Density Converter

**Category Name**: `heatDensity`

**Base Unit (SI Derived Unit)**: joule per cubic meter (J/m³) ⭐

**Description**: Heat density is the amount of heat energy per unit volume. The SI derived unit is joule per cubic meter (J/m³).

**Common Use Cases**:
- Energy storage
- Thermal analysis
- Material properties
- Engineering calculations

**Unit Systems**:
- **SI**: joule per cubic meter (J/m³), kilojoule per cubic meter (kJ/m³), megajoule per cubic meter (MJ/m³)
- **Imperial/US Customary**: BTU per cubic foot (BTU/ft³), calorie per cubic centimeter (cal/cm³)
- **Other**: therm per cubic foot (therm/ft³)

#### 2.6.9 Heat Flux Density Converter

**Category Name**: `heatFluxDensity`

**Base Unit (SI Derived Unit)**: watt per square meter (W/m²) = kg/s³ ⭐

**Description**: Heat flux density is the rate of heat transfer per unit area. The SI derived unit is watt per square meter (W/m²).

**Common Use Cases**:
- Heat transfer calculations
- Solar radiation
- Thermal analysis
- Engineering design

**Unit Systems**:
- **SI**: watt per square meter (W/m²), kilowatt per square meter (kW/m²)
- **Imperial/US Customary**: BTU per hour square foot (BTU/(h·ft²)), calorie per second square centimeter (cal/(s·cm²))
- **Other**: erg per second square centimeter (erg/(s·cm²)), langley per minute (ly/min)

#### 2.6.10 Heat Transfer Coefficient Converter

**Category Name**: `heatTransferCoefficient`

**Base Unit (SI Derived Unit)**: watt per square meter kelvin (W/(m²·K)) = kg/(s³·K) ⭐

**Description**: Heat transfer coefficient measures the rate of heat transfer per unit area per unit temperature difference. The SI derived unit is watt per square meter kelvin (W/(m²·K)).

**Common Use Cases**:
- Heat transfer calculations
- HVAC design
- Thermal analysis
- Engineering specifications

**Unit Systems**:
- **SI**: watt per square meter kelvin (W/(m²·K)), kilowatt per square meter kelvin (kW/(m²·K))
- **Imperial/US Customary**: BTU per hour square foot degree Fahrenheit (BTU/(h·ft²·°F)), calorie per second square centimeter degree Celsius (cal/(s·cm²·°C))
- **Other**: kilocalorie per hour square meter degree Celsius (kcal/(h·m²·°C))

---

## 3. Implementation Requirements

### 3.1 JSON Configuration Files

#### 3.1.1 File Structure

Create one JSON configuration file for each new category in the `UnitsSettings` folder:

```
UnitsSettings/
├── pressure.json
├── energy.json
├── power.json
├── charge.json
├── linearChargeDensity.json
├── surfaceChargeDensity.json
├── volumeChargeDensity.json
├── current.json
├── linearCurrentDensity.json
├── surfaceCurrentDensity.json
├── electricFieldStrength.json
├── electricPotential.json
├── electricResistance.json
├── electricResistivity.json
├── electricConductance.json
├── electricConductivity.json
├── capacitance.json
├── inductance.json
├── angularVelocity.json
├── acceleration.json
├── angularAcceleration.json
├── density.json
├── specificVolume.json
├── momentOfInertia.json
├── momentOfForce.json
├── torque.json
├── fuelEfficiencyMass.json
├── fuelEfficiencyVolume.json
├── temperatureInterval.json
├── thermalExpansion.json
├── thermalResistance.json
├── thermalConductivity.json
├── specificHeatCapacity.json
├── heatDensity.json
├── heatFluxDensity.json
└── heatTransferCoefficient.json
```

#### 3.1.2 JSON File Format

Each JSON file must follow the same structure as existing category files:

```json
{
  "category": "pressure",
  "categoryDisplayName": "Pressure",
  "group": "Common",
  "baseUnit": {
    "symbol": "Pa",
    "name": "pascal",
    "displayName": "Pascal",
    "isBaseUnit": true,
    "isSIUnit": true,
    "unitSystem": "SI"
  },
  "units": [
    {
      "symbol": "Pa",
      "name": "pascal",
      "displayName": "Pascal",
      "category": "pressure",
      "isBaseUnit": true,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 1.0,
      "conversionFormula": null
    },
    {
      "symbol": "kPa",
      "name": "kilopascal",
      "displayName": "Kilopascal",
      "category": "pressure",
      "isBaseUnit": false,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 1000.0,
      "conversionFormula": null
    }
  ]
}
```

#### 3.1.3 Group Property

Each JSON file must include a `"group"` property to organize categories in the UI:

- **Common**: Basic everyday converters (length, weight, volume, temperature, area, time, speed, pressure, energy, power)
- **Engineering**: Engineering and mechanical converters (angularVelocity, acceleration, angularAcceleration, density, specificVolume, momentOfInertia, momentOfForce, torque)
- **Electricity**: Electrical and electromagnetic converters (charge, linearChargeDensity, surfaceChargeDensity, volumeChargeDensity, current, linearCurrentDensity, surfaceCurrentDensity, electricFieldStrength, electricPotential, electricResistance, electricResistivity, electricConductance, electricConductivity, capacitance, inductance)
- **Heat**: Thermal and heat-related converters (fuelEfficiencyMass, fuelEfficiencyVolume, temperatureInterval, thermalExpansion, thermalResistance, thermalConductivity, specificHeatCapacity, heatDensity, heatFluxDensity, heatTransferCoefficient)

**Default Value**: If the `group` property is omitted, it defaults to `"Common"`.

#### 3.1.4 Conversion Factors

- All conversion factors must be relative to the base unit (SI unit)
- For linear conversions: use `conversionFactor`
- For formula-based conversions: use `conversionFormula` and `conversionInverseFormula`
- Ensure all conversion factors are accurate and verified

### 3.2 Backend Implementation

#### 3.2.1 Backend Code Changes

The backend architecture has been updated to support the group feature:

- **Infrastructure Layer**: 
  - `UnitCategoryJson` data model includes optional `Group` property
  - `JsonUnitRepository` reads the `group` property from JSON files and defaults to `"Common"` if not specified
- **Domain Layer**: 
  - `Category` entity includes `Group` property
  - `ConversionService` handles conversions generically for all categories (no changes needed)
- **Application Layer**: 
  - `CategoryDto` includes `Group` property
  - `ConversionMapping.ToCategoryDto()` maps the group property
  - `UnitConverterService` orchestrates conversions for any category (no changes needed)
- **Presentation Layer**: 
  - Controllers return categories with group information dynamically
  - API documentation updated to reflect group organization

#### 3.2.2 Verification Steps

1. **Startup Loading**: Verify all new JSON files are loaded at application startup
2. **Category Listing**: Verify new categories appear in `GET /api/categories` endpoint
3. **Unit Listing**: Verify units appear in `GET /api/categories/{name}/units` endpoint
4. **Conversion**: Verify conversions work correctly for all new categories
5. **Error Handling**: Verify proper error messages for invalid categories/units

#### 3.2.3 Testing Requirements

- Unit tests for conversion accuracy (≥95% coverage maintained)
- Integration tests for all new API endpoints
- Edge case testing (very large/small values)
- Cross-category validation (ensure units from different categories cannot be converted)

### 3.3 Frontend Implementation

#### 3.3.1 Category Grouping

The frontend implements a group-based organization system:

- **Group Selection Radio Buttons**: 
  - Radio buttons allow users to filter categories by group (All, Common, Engineering, Electricity, Heat)
  - Groups are dynamically extracted from category data
  - Default selection is "All" to show all categories
- **Group Filtering**: 
  - Categories are filtered by the selected group
  - Search functionality works within the selected group
  - When a group is selected, the category search is cleared for better UX
- **Group Display**: 
  - Group names are localized using translation keys (`groups.all`, `groups.common`, `groups.engineering`, `groups.electricity`, `groups.heat`)
  - Radio buttons are styled with hover and focus states for accessibility
  - Responsive design: radio buttons stack vertically on mobile devices

#### 3.3.2 Category Display

- New categories must appear in the category selector dropdown
- Categories are filtered by the selected group
- Categories are sorted alphabetically by display name within each group
- Category names must be properly localized

#### 3.3.2 Unit Selection

- Unit dropdowns must populate correctly for each new category
- Unit symbols and display names must be shown correctly
- SI unit indicators (⭐) must display for base units
- Unit system badges (SI, Imperial, etc.) must display correctly

#### 3.3.3 Conversion Display

- Conversion results must display correctly
- Large/small numbers must be formatted appropriately
- Scientific notation should be used when appropriate
- Result precision should be controlled (e.g., 4-6 decimal places)

#### 3.3.4 UI/UX Considerations

- Ensure responsive design works for all new categories
- Verify mobile layout accommodates longer unit names
- Test language switching with new categories
- Verify accessibility (screen readers, keyboard navigation)

### 3.4 Localization

#### 3.4.1 Translation Files

Update translation files for all supported languages (English, Chinese, French):

**Frontend Translation Files**:
- `frontend/src/i18n/locales/en.json`
- `frontend/src/i18n/locales/zh.json`
- `frontend/src/i18n/locales/fr.json`

**Backend Resource Files**:
- `SolutionDotnetReact/src/UCConverter.Application/Resources/SharedResources.en.resx`
- `SolutionDotnetReact/src/UCConverter.Application/Resources/SharedResources.zh.resx`
- `SolutionDotnetReact/src/UCConverter.Application/Resources/SharedResources.fr.resx`

#### 3.4.2 Translation Coverage

All new categories and units must be translated:

- **Category Names**: 
  - Pressure → 压力 (Chinese) → Pression (French)
  - Energy → 能量 (Chinese) → Énergie (French)
  - Power → 功率 (Chinese) → Puissance (French)
  - All electricity category names
  - Angular Velocity → 角速度 (Chinese) → Vitesse Angulaire (French)
  - Acceleration → 加速度 (Chinese) → Accélération (French)
  - Density → 密度 (Chinese) → Densité (French)
  - Torque → 扭矩 (Chinese) → Couple (French)
  - Thermal Conductivity → 热导率 (Chinese) → Conductivité Thermique (French)
  - All other engineering and heat category names

- **Unit Names**: All unit display names must be translated
- **Unit Symbols**: Symbols typically remain unchanged (Pa, J, W, etc.)
- **Error Messages**: Any category-specific error messages must be translated

#### 3.4.3 Translation Keys

Follow existing translation key patterns:

**Category Translations**:
```json
{
  "categories": {
    "pressure": "Pressure",
    "energy": "Energy",
    "power": "Power",
    "charge": "Charge",
    "current": "Current",
    "electricPotential": "Electric Potential",
    "electricResistance": "Electric Resistance",
    "capacitance": "Capacitance",
    "inductance": "Inductance",
    "angularVelocity": "Angular Velocity",
    "acceleration": "Acceleration",
    "angularAcceleration": "Angular Acceleration",
    "density": "Density",
    "specificVolume": "Specific Volume",
    "momentOfInertia": "Moment of Inertia",
    "momentOfForce": "Moment of Force",
    "torque": "Torque",
    "fuelEfficiencyMass": "Fuel Efficiency - Mass",
    "fuelEfficiencyVolume": "Fuel Efficiency - Volume",
    "temperatureInterval": "Temperature Interval",
    "thermalExpansion": "Thermal Expansion",
    "thermalResistance": "Thermal Resistance",
    "thermalConductivity": "Thermal Conductivity",
    "specificHeatCapacity": "Specific Heat Capacity",
    "heatDensity": "Heat Density",
    "heatFluxDensity": "Heat Flux Density",
    "heatTransferCoefficient": "Heat Transfer Coefficient"
  }
}
```

**Group Translations** (New):
```json
{
  "groups": {
    "all": "All",
    "common": "Common",
    "engineering": "Engineering",
    "electricity": "Electricity",
    "heat": "Heat"
  }
}
```

**Unit Translations**:
```json
{
  "units": {
    "pascal": "Pascal",
    "joule": "Joule",
    "watt": "Watt",
    "coulomb": "Coulomb",
    "ampere": "Ampere",
    "volt": "Volt",
    "ohm": "Ohm",
    "farad": "Farad",
    "henry": "Henry",
    "radianPerSecond": "Radian per Second",
    "meterPerSecondSquared": "Meter per Second Squared",
    "kilogramPerCubicMeter": "Kilogram per Cubic Meter",
    "newtonMeter": "Newton Meter"
  }
}
```

**Group Translation Examples**:
- **English**: All, Common, Engineering, Electricity, Heat
- **Chinese**: 全部, 常用, 工程, 电气, 热学
- **French**: Tous, Commun, Ingénierie, Électricité, Thermique

---

## 4. Unit Definitions

### 4.1 Pressure Units

**Base Unit**: pascal (Pa) = N/m² = kg/(m·s²)

**Common Units**:
- **SI Units**:
  - pascal (Pa) - base unit
  - kilopascal (kPa) = 1,000 Pa
  - megapascal (MPa) = 1,000,000 Pa
  - gigapascal (GPa) = 1,000,000,000 Pa
- **Non-SI Metric**:
  - bar = 100,000 Pa
  - millibar (mbar) = 100 Pa
  - kilobar (kbar) = 100,000,000 Pa
- **Imperial/US Customary**:
  - pound per square inch (psi) = 6,894.76 Pa
  - pound per square foot (psf) = 47.8803 Pa
  - kip per square inch (ksi) = 6,894,760 Pa
- **Other**:
  - atmosphere (atm) = 101,325 Pa
  - technical atmosphere (at) = 98,066.5 Pa
  - torr = 133.322 Pa
  - millimeter of mercury (mmHg) = 133.322 Pa
  - inch of mercury (inHg) = 3,386.39 Pa
  - inch of water (inH₂O) = 249.089 Pa

### 4.2 Energy Units

**Base Unit**: joule (J) = N·m = kg·m²/s²

**Common Units**:
- **SI Units**:
  - joule (J) - base unit
  - kilojoule (kJ) = 1,000 J
  - megajoule (MJ) = 1,000,000 J
  - gigajoule (GJ) = 1,000,000,000 J
- **Non-SI Metric**:
  - calorie (cal) = 4.184 J
  - kilocalorie (kcal) = 4,184 J
  - watt-hour (W·h) = 3,600 J
  - kilowatt-hour (kW·h) = 3,600,000 J
- **Imperial/US Customary**:
  - British Thermal Unit (BTU) = 1,055.06 J
  - therm = 105,506,000 J
  - foot-pound (ft·lb) = 1.35582 J
  - foot-poundal = 0.0421401 J
- **Other**:
  - electronvolt (eV) = 1.602176634 × 10⁻¹⁹ J
  - kiloelectronvolt (keV) = 1.602176634 × 10⁻¹⁶ J
  - megaelectronvolt (MeV) = 1.602176634 × 10⁻¹³ J
  - quad = 1.055 × 10¹⁸ J

### 4.3 Power Units

**Base Unit**: watt (W) = J/s = kg·m²/s³

**Common Units**:
- **SI Units**:
  - watt (W) - base unit
  - kilowatt (kW) = 1,000 W
  - megawatt (MW) = 1,000,000 W
  - gigawatt (GW) = 1,000,000,000 W
  - terawatt (TW) = 1,000,000,000,000 W
- **Imperial/US Customary**:
  - horsepower (hp) = 745.7 W
  - metric horsepower (PS) = 735.499 W
  - foot-pound per second (ft·lb/s) = 1.35582 W
  - British Thermal Unit per hour (BTU/h) = 0.293071 W
  - ton of refrigeration (TR) = 3,516.85 W
- **Other**:
  - erg per second (erg/s) = 10⁻⁷ W
  - calorie per second (cal/s) = 4.184 W
  - kilocalorie per hour (kcal/h) = 1.163 W

### 4.4 Electricity Units

#### 4.4.1 Charge Units

**Base Unit**: coulomb (C) = A·s

**Common Units**:
- **SI Units**:
  - coulomb (C) - base unit
  - millicoulomb (mC) = 0.001 C
  - microcoulomb (µC) = 0.000001 C
  - nanocoulomb (nC) = 10⁻⁹ C
  - picocoulomb (pC) = 10⁻¹² C
- **CGS Units**:
  - statcoulomb (statC) = 3.33564 × 10⁻¹⁰ C
  - franklin (Fr) = 3.33564 × 10⁻¹⁰ C
- **Other**:
  - ampere-hour (A·h) = 3,600 C
  - milliampere-hour (mA·h) = 3.6 C
  - faraday (F) = 96,485.3 C

#### 4.4.2 Current Units

**Base Unit**: ampere (A)

**Common Units**:
- **SI Units**:
  - ampere (A) - base unit
  - milliampere (mA) = 0.001 A
  - microampere (µA) = 0.000001 A
  - nanoampere (nA) = 10⁻⁹ A
  - kiloampere (kA) = 1,000 A
- **CGS Units**:
  - statampere (statA) = 3.33564 × 10⁻¹⁰ A
  - abampere (abA) = 10 A
  - biot (Bi) = 10 A

#### 4.4.3 Electric Potential (Voltage) Units

**Base Unit**: volt (V) = J/C = kg·m²/(A·s³)

**Common Units**:
- **SI Units**:
  - volt (V) - base unit
  - millivolt (mV) = 0.001 V
  - microvolt (µV) = 0.000001 V
  - kilovolt (kV) = 1,000 V
  - megavolt (MV) = 1,000,000 V
- **CGS Units**:
  - statvolt (statV) = 299.792 V
  - abvolt (abV) = 10⁻⁸ V

#### 4.4.4 Electric Resistance Units

**Base Unit**: ohm (Ω) = V/A = kg·m²/(A²·s³)

**Common Units**:
- **SI Units**:
  - ohm (Ω) - base unit
  - milliohm (mΩ) = 0.001 Ω
  - microohm (µΩ) = 0.000001 Ω
  - kiloohm (kΩ) = 1,000 Ω
  - megaohm (MΩ) = 1,000,000 Ω
  - gigaohm (GΩ) = 1,000,000,000 Ω
- **CGS Units**:
  - statohm = 8.98755 × 10¹¹ Ω
  - abohm = 10⁻⁹ Ω

#### 4.4.5 Capacitance Units

**Base Unit**: farad (F) = C/V = A²·s⁴/(kg·m²)

**Common Units**:
- **SI Units**:
  - farad (F) - base unit
  - millifarad (mF) = 0.001 F
  - microfarad (µF) = 0.000001 F
  - nanofarad (nF) = 10⁻⁹ F
  - picofarad (pF) = 10⁻¹² F
- **CGS Units**:
  - statfarad (statF) = 1.11265 × 10⁻¹² F
  - abfarad (abF) = 10⁹ F

#### 4.4.6 Inductance Units

**Base Unit**: henry (H) = V·s/A = kg·m²/(A²·s²)

**Common Units**:
- **SI Units**:
  - henry (H) - base unit
  - millihenry (mH) = 0.001 H
  - microhenry (µH) = 0.000001 H
  - nanohenry (nH) = 10⁻⁹ H
  - picohenry (pH) = 10⁻¹² H
- **CGS Units**:
  - abhenry (abH) = 10⁻⁹ H
  - stathenry (statH) = 8.98755 × 10¹¹ H

#### 4.4.7 Other Electricity Units

**Linear Charge Density**: coulomb per meter (C/m)
**Surface Charge Density**: coulomb per square meter (C/m²)
**Volume Charge Density**: coulomb per cubic meter (C/m³)
**Linear Current Density**: ampere per meter (A/m)
**Surface Current Density**: ampere per meter (A/m)
**Electric Field Strength**: volt per meter (V/m)
**Electric Resistivity**: ohm meter (Ω·m)
**Electric Conductance**: siemens (S) = 1/Ω
**Electric Conductivity**: siemens per meter (S/m)

### 4.5 Engineering Units

#### 4.5.1 Angular Velocity Units

**Base Unit**: radian per second (rad/s)

**Common Units**:
- **SI Units**:
  - radian per second (rad/s) - base unit
  - radian per minute (rad/min) = 0.10472 rad/s
- **Non-SI**:
  - degree per second (°/s) = 0.0174533 rad/s
  - degree per minute (°/min) = 0.000290888 rad/s
  - revolution per second (rps) = 6.28319 rad/s
  - revolution per minute (rpm) = 0.10472 rad/s
  - revolution per hour (rph) = 0.00174533 rad/s

#### 4.5.2 Acceleration Units

**Base Unit**: meter per second squared (m/s²)

**Common Units**:
- **SI Units**:
  - meter per second squared (m/s²) - base unit
  - kilometer per hour squared (km/h²) = 0.0000771605 m/s²
- **Imperial/US Customary**:
  - foot per second squared (ft/s²) = 0.3048 m/s²
  - inch per second squared (in/s²) = 0.0254 m/s²
- **Other**:
  - standard gravity (g) = 9.80665 m/s²
  - gal (Gal) = 0.01 m/s²
  - milligal (mGal) = 0.00001 m/s²

#### 4.5.3 Angular Acceleration Units

**Base Unit**: radian per second squared (rad/s²)

**Common Units**:
- **SI Units**:
  - radian per second squared (rad/s²) - base unit
- **Non-SI**:
  - degree per second squared (°/s²) = 0.0174533 rad/s²
  - revolution per second squared (rps²) = 39.4784 rad/s²
  - revolution per minute squared (rpm²) = 0.00174533 rad/s²

#### 4.5.4 Density Units

**Base Unit**: kilogram per cubic meter (kg/m³)

**Common Units**:
- **SI Units**:
  - kilogram per cubic meter (kg/m³) - base unit
  - gram per cubic centimeter (g/cm³) = 1,000 kg/m³
  - gram per liter (g/L) = 1 kg/m³
  - kilogram per liter (kg/L) = 1,000 kg/m³
- **Imperial/US Customary**:
  - pound per cubic foot (lb/ft³) = 16.0185 kg/m³
  - pound per cubic inch (lb/in³) = 27,679.9 kg/m³
  - ounce per cubic inch (oz/in³) = 1,729.99 kg/m³
- **Other**:
  - slug per cubic foot (slug/ft³) = 515.379 kg/m³

#### 4.5.5 Specific Volume Units

**Base Unit**: cubic meter per kilogram (m³/kg)

**Common Units**:
- **SI Units**:
  - cubic meter per kilogram (m³/kg) - base unit
  - liter per kilogram (L/kg) = 0.001 m³/kg
  - cubic centimeter per gram (cm³/g) = 0.001 m³/kg
- **Imperial/US Customary**:
  - cubic foot per pound (ft³/lb) = 0.062428 m³/kg
  - cubic inch per pound (in³/lb) = 0.0000361273 m³/kg
  - gallon per pound (gal/lb) = 0.0083454 m³/kg

#### 4.5.6 Moment of Inertia Units

**Base Unit**: kilogram square meter (kg·m²)

**Common Units**:
- **SI Units**:
  - kilogram square meter (kg·m²) - base unit
  - gram square centimeter (g·cm²) = 0.0000001 kg·m²
- **Imperial/US Customary**:
  - pound square foot (lb·ft²) = 0.0421401 kg·m²
  - pound square inch (lb·in²) = 0.00029264 kg·m²
  - slug square foot (slug·ft²) = 1.35582 kg·m²
- **Other**:
  - ounce square inch (oz·in²) = 0.00001829 kg·m²

#### 4.5.7 Moment of Force / Torque Units

**Base Unit**: newton meter (N·m) = kg·m²/s²

**Common Units**:
- **SI Units**:
  - newton meter (N·m) - base unit
  - kilonewton meter (kN·m) = 1,000 N·m
  - newton centimeter (N·cm) = 0.01 N·m
  - newton millimeter (N·mm) = 0.001 N·m
- **Imperial/US Customary**:
  - pound-force foot (lbf·ft) = 1.35582 N·m
  - pound-force inch (lbf·in) = 0.112985 N·m
  - ounce-force inch (ozf·in) = 0.00706155 N·m
- **Other**:
  - kilogram-force meter (kgf·m) = 9.80665 N·m
  - dyne centimeter (dyn·cm) = 0.0000001 N·m

### 4.6 Heat Units

#### 4.6.1 Fuel Efficiency - Mass Units

**Base Unit**: meter per kilogram (m/kg)

**Common Units**:
- **SI Units**:
  - meter per kilogram (m/kg) - base unit
  - kilometer per kilogram (km/kg) = 1,000 m/kg
- **Imperial/US Customary**:
  - mile per pound (mi/lb) = 3,218.69 m/kg
  - foot per pound (ft/lb) = 0.671969 m/kg
- **Other**:
  - nautical mile per pound (nmi/lb) = 3,728.15 m/kg

#### 4.6.2 Fuel Efficiency - Volume Units

**Base Unit**: meter per cubic meter (m/m³)

**Common Units**:
- **SI Units**:
  - meter per cubic meter (m/m³) - base unit
  - kilometer per liter (km/L) = 1,000,000 m/m³
  - liter per 100 kilometers (L/100km) = 0.00001 m/m³
- **Imperial/US Customary**:
  - mile per gallon (US) (mpg US) = 425,143.7 m/m³
  - mile per gallon (UK) (mpg UK) = 354,006.2 m/m³
- **Other**:
  - nautical mile per gallon (nmi/gal) = 489,575.5 m/m³

#### 4.6.3 Temperature Interval Units

**Base Unit**: kelvin (K)

**Common Units**:
- **SI Units**:
  - kelvin (K) - base unit
  - degree Celsius (°C) - for intervals, 1 K = 1 °C
- **Non-SI**:
  - degree Fahrenheit (°F) - for intervals, 1 K = 1.8 °F
  - degree Rankine (°R) - for intervals, 1 K = 1.8 °R

#### 4.6.4 Thermal Expansion Units

**Base Unit**: per kelvin (1/K)

**Common Units**:
- **SI Units**:
  - per kelvin (1/K) - base unit
  - per degree Celsius (1/°C) = 1 1/K
- **Non-SI**:
  - per degree Fahrenheit (1/°F) = 1.8 1/K
  - per degree Rankine (1/°R) = 1.8 1/K

#### 4.6.5 Thermal Resistance Units

**Base Unit**: kelvin per watt (K/W)

**Common Units**:
- **SI Units**:
  - kelvin per watt (K/W) - base unit
  - degree Celsius per watt (°C/W) = 1 K/W
  - square meter kelvin per watt (m²·K/W) - for area-specific
- **Imperial/US Customary**:
  - hour square foot degree Fahrenheit per BTU (h·ft²·°F/BTU) = 0.17611 m²·K/W

#### 4.6.6 Thermal Conductivity Units

**Base Unit**: watt per meter kelvin (W/(m·K))

**Common Units**:
- **SI Units**:
  - watt per meter kelvin (W/(m·K)) - base unit
  - watt per centimeter kelvin (W/(cm·K)) = 100 W/(m·K)
- **Imperial/US Customary**:
  - BTU per hour foot degree Fahrenheit (BTU/(h·ft·°F)) = 1.73073 W/(m·K)
  - calorie per second centimeter degree Celsius (cal/(s·cm·°C)) = 418.4 W/(m·K)
- **Other**:
  - kilocalorie per hour meter degree Celsius (kcal/(h·m·°C)) = 1.163 W/(m·K)

#### 4.6.7 Specific Heat Capacity Units

**Base Unit**: joule per kilogram kelvin (J/(kg·K))

**Common Units**:
- **SI Units**:
  - joule per kilogram kelvin (J/(kg·K)) - base unit
  - kilojoule per kilogram kelvin (kJ/(kg·K)) = 1,000 J/(kg·K)
- **Imperial/US Customary**:
  - BTU per pound degree Fahrenheit (BTU/(lb·°F)) = 4,186.8 J/(kg·K)
  - calorie per gram degree Celsius (cal/(g·°C)) = 4,184 J/(kg·K)
- **Other**:
  - kilocalorie per kilogram degree Celsius (kcal/(kg·°C)) = 4,184 J/(kg·K)

#### 4.6.8 Heat Density Units

**Base Unit**: joule per cubic meter (J/m³)

**Common Units**:
- **SI Units**:
  - joule per cubic meter (J/m³) - base unit
  - kilojoule per cubic meter (kJ/m³) = 1,000 J/m³
  - megajoule per cubic meter (MJ/m³) = 1,000,000 J/m³
- **Imperial/US Customary**:
  - BTU per cubic foot (BTU/ft³) = 37,258.9 J/m³
  - calorie per cubic centimeter (cal/cm³) = 4,184,000 J/m³
- **Other**:
  - therm per cubic foot (therm/ft³) = 3,725,890,000 J/m³

#### 4.6.9 Heat Flux Density Units

**Base Unit**: watt per square meter (W/m²)

**Common Units**:
- **SI Units**:
  - watt per square meter (W/m²) - base unit
  - kilowatt per square meter (kW/m²) = 1,000 W/m²
- **Imperial/US Customary**:
  - BTU per hour square foot (BTU/(h·ft²)) = 3.15459 W/m²
  - calorie per second square centimeter (cal/(s·cm²)) = 41,840 W/m²
- **Other**:
  - erg per second square centimeter (erg/(s·cm²)) = 0.001 W/m²
  - langley per minute (ly/min) = 697.333 W/m²

#### 4.6.10 Heat Transfer Coefficient Units

**Base Unit**: watt per square meter kelvin (W/(m²·K))

**Common Units**:
- **SI Units**:
  - watt per square meter kelvin (W/(m²·K)) - base unit
  - kilowatt per square meter kelvin (kW/(m²·K)) = 1,000 W/(m²·K)
- **Imperial/US Customary**:
  - BTU per hour square foot degree Fahrenheit (BTU/(h·ft²·°F)) = 5.67826 W/(m²·K)
  - calorie per second square centimeter degree Celsius (cal/(s·cm²·°C)) = 41,840 W/(m²·K)
- **Other**:
  - kilocalorie per hour square meter degree Celsius (kcal/(h·m²·°C)) = 1.163 W/(m²·K)

---

## 5. Implementation Priorities

### Priority 1 (High) - Core Categories

1. **Pressure Converter**
   - Create `pressure.json` configuration file
   - Add common units (Pa, kPa, bar, psi, atm)
   - Test conversions
   - Add translations

2. **Energy Converter**
   - Create `energy.json` configuration file
   - Add common units (J, kJ, cal, kcal, kWh, BTU)
   - Test conversions
   - Add translations

3. **Power Converter**
   - Create `power.json` configuration file
   - Add common units (W, kW, MW, hp, BTU/h)
   - Test conversions
   - Add translations

### Priority 2 (Medium) - Essential Electricity Converters

1. **Charge Converter**
   - Create `charge.json` configuration file
   - Add common units (C, mC, µC, A·h, mA·h)
   - Test conversions
   - Add translations

2. **Current Converter**
   - Create `current.json` configuration file
   - Add common units (A, mA, µA, kA)
   - Test conversions
   - Add translations

3. **Electric Potential Converter**
   - Create `electricPotential.json` configuration file
   - Add common units (V, mV, kV, MV)
   - Test conversions
   - Add translations

4. **Electric Resistance Converter**
   - Create `electricResistance.json` configuration file
   - Add common units (Ω, mΩ, kΩ, MΩ)
   - Test conversions
   - Add translations

5. **Capacitance Converter**
   - Create `capacitance.json` configuration file
   - Add common units (F, mF, µF, nF, pF)
   - Test conversions
   - Add translations

6. **Inductance Converter**
   - Create `inductance.json` configuration file
   - Add common units (H, mH, µH, nH)
   - Test conversions
   - Add translations

### Priority 3 (Medium) - Engineering Converters

1. **Density Converter**
   - Create `density.json` configuration file
   - Add common units (kg/m³, g/cm³, lb/ft³)
   - Test conversions
   - Add translations

2. **Acceleration Converter**
   - Create `acceleration.json` configuration file
   - Add common units (m/s², ft/s², g)
   - Test conversions
   - Add translations

3. **Torque / Moment of Force Converter**
   - Create `torque.json` and `momentOfForce.json` configuration files
   - Add common units (N·m, lbf·ft, kgf·m)
   - Test conversions
   - Add translations

4. **Angular Velocity Converter**
   - Create `angularVelocity.json` configuration file
   - Add common units (rad/s, rpm, °/s)
   - Test conversions
   - Add translations

5. **Specific Volume Converter**
   - Create `specificVolume.json` configuration file
   - Add common units (m³/kg, ft³/lb)
   - Test conversions
   - Add translations

6. **Angular Acceleration Converter**
   - Create `angularAcceleration.json` configuration file
   - Add common units (rad/s², °/s²)
   - Test conversions
   - Add translations

7. **Moment of Inertia Converter**
   - Create `momentOfInertia.json` configuration file
   - Add common units (kg·m², lb·ft²)
   - Test conversions
   - Add translations

### Priority 4 (Medium) - Heat Converters

1. **Thermal Conductivity Converter**
   - Create `thermalConductivity.json` configuration file
   - Add common units (W/(m·K), BTU/(h·ft·°F))
   - Test conversions
   - Add translations

2. **Specific Heat Capacity Converter**
   - Create `specificHeatCapacity.json` configuration file
   - Add common units (J/(kg·K), BTU/(lb·°F))
   - Test conversions
   - Add translations

3. **Heat Flux Density Converter**
   - Create `heatFluxDensity.json` configuration file
   - Add common units (W/m², BTU/(h·ft²))
   - Test conversions
   - Add translations

4. **Heat Transfer Coefficient Converter**
   - Create `heatTransferCoefficient.json` configuration file
   - Add common units (W/(m²·K), BTU/(h·ft²·°F))
   - Test conversions
   - Add translations

5. **Fuel Efficiency - Volume Converter**
   - Create `fuelEfficiencyVolume.json` configuration file
   - Add common units (km/L, mpg, L/100km)
   - Test conversions
   - Add translations

6. **Thermal Resistance Converter**
   - Create `thermalResistance.json` configuration file
   - Add common units (K/W, m²·K/W)
   - Test conversions
   - Add translations

7. **Heat Density Converter**
   - Create `heatDensity.json` configuration file
   - Add common units (J/m³, BTU/ft³)
   - Test conversions
   - Add translations

8. **Fuel Efficiency - Mass Converter**
   - Create `fuelEfficiencyMass.json` configuration file
   - Add common units (m/kg, mi/lb)
   - Test conversions
   - Add translations

9. **Temperature Interval Converter**
   - Create `temperatureInterval.json` configuration file
   - Add common units (K, °C, °F for intervals)
   - Test conversions
   - Add translations

10. **Thermal Expansion Converter**
    - Create `thermalExpansion.json` configuration file
    - Add common units (1/K, 1/°C, 1/°F)
    - Test conversions
    - Add translations

### Priority 5 (Low) - Advanced Electricity Converters

1. **Charge Density Converters**
   - Linear Charge Density
   - Surface Charge Density
   - Volume Charge Density

2. **Current Density Converters**
   - Linear Current Density
   - Surface Current Density

3. **Field and Material Property Converters**
   - Electric Field Strength
   - Electric Resistivity
   - Electric Conductance
   - Electric Conductivity

---

## 6. Success Criteria

### 6.1 Backend Implementation

- [ ] All 36 new JSON configuration files created in `UnitsSettings` folder
- [ ] All JSON files follow the correct structure and format
- [ ] All JSON files include the `group` property (Common, Engineering, Electricity, or Heat)
- [ ] All conversion factors are accurate and verified
- [ ] All categories appear in `GET /api/categories` endpoint with group information
- [ ] All units appear in `GET /api/categories/{name}/units` endpoint
- [ ] All conversions work correctly via `POST /api/convert` endpoint
- [ ] Error handling works for invalid categories/units
- [ ] All new categories load successfully at application startup
- [ ] Group property defaults to "Common" if not specified in JSON
- [ ] No breaking changes to existing functionality
- [ ] ≥95% code coverage maintained for all layers

### 6.2 Frontend Implementation

- [ ] Group selection radio buttons display correctly (All, Common, Engineering, Electricity, Heat)
- [ ] Categories are filtered by selected group
- [ ] Group names are properly localized
- [ ] All new categories appear in category selector
- [ ] Categories are sorted alphabetically within each group
- [ ] Category search works within selected group
- [ ] All units display correctly in unit dropdowns
- [ ] Conversions work correctly for all new categories
- [ ] Results display with appropriate formatting
- [ ] SI unit indicators (⭐) display correctly
- [ ] Unit system badges display correctly
- [ ] Responsive design works for all new categories
- [ ] Mobile layout accommodates new categories (radio buttons stack vertically)
- [ ] Accessibility maintained (screen readers, keyboard navigation, ARIA labels)

### 6.3 Localization

- [ ] All category names translated in English, Chinese, and French
- [ ] All group names translated in English, Chinese, and French (All, Common, Engineering, Electricity, Heat)
- [ ] All unit names translated in English, Chinese, and French
- [ ] Error messages translated for all languages
- [ ] Language switching works correctly with new categories
- [ ] Group names update correctly when language changes
- [ ] API returns localized content when `locale` parameter is used
- [ ] Frontend displays localized content correctly

### 6.4 Testing

- [ ] Unit tests for all new conversion calculations
- [ ] Integration tests for all new API endpoints
- [ ] Edge case testing (very large/small values)
- [ ] Cross-category validation (units from different categories cannot be converted)
- [ ] Localization testing (all languages)
- [ ] Frontend testing (all new categories)
- [ ] Mobile device testing
- [ ] Cross-browser testing

### 6.5 Documentation

- [ ] API documentation updated (Swagger) with new categories and group information
- [ ] README updated to mention new categories and group feature
- [ ] Unit conversion examples added for new categories
- [ ] Group feature documented in user-facing documentation
- [ ] Translation guidelines updated (if needed)

---

## 7. Testing Requirements

### 7.1 Unit Testing

#### 7.1.1 Conversion Accuracy Tests

For each new category, test:
- Conversion from base unit to all other units
- Conversion from all units to base unit
- Conversion between non-base units
- Very large values (e.g., 1,000,000)
- Very small values (e.g., 0.000001)
- Zero values
- Negative values (where applicable)

#### 7.1.2 Edge Cases

- Maximum/minimum representable values
- Precision limits
- Rounding behavior
- Scientific notation handling

### 7.2 Integration Testing

#### 7.2.1 API Endpoint Testing

For each new category:
- `GET /api/categories` - verify category appears
- `GET /api/categories/{name}/units` - verify all units appear
- `POST /api/convert` - verify conversions work correctly
- Test with different locales (`?locale=en`, `?locale=zh`, `?locale=fr`)
- Test error scenarios (invalid category, invalid unit, etc.)

#### 7.2.2 End-to-End Testing

- Complete conversion flow: select category → select units → enter value → get result
- Language switching with new categories
- Mobile device testing
- Cross-browser testing

### 7.3 Validation Testing

- Verify units from different categories cannot be converted
- Verify invalid unit symbols are rejected
- Verify invalid categories are rejected
- Verify proper error messages are returned

### 7.4 Performance Testing

- Verify startup time is acceptable with 18 new JSON files
- Verify API response times remain <50ms
- Verify memory usage is acceptable
- Verify no memory leaks

### 7.5 Localization Testing

- Test all categories in English
- Test all categories in Chinese
- Test all categories in French
- Verify translations are accurate
- Verify no missing translations
- Test language switching

---

## 8. Technical Considerations

### 8.1 Conversion Factor Accuracy

- All conversion factors must be accurate to at least 6 significant figures
- Use standard reference values (NIST, BIPM)
- Document sources for conversion factors
- Verify conversions with known test cases

### 8.2 Unit Symbol Consistency

- Follow standard unit symbol conventions (SI, ISO)
- Ensure symbols are unique within each category
- Handle special characters correctly (Ω, µ, °, etc.)
- Support Unicode symbols where needed

### 8.3 Scientific Notation

- For very large/small values, use scientific notation in results
- Format: `1.234 × 10⁶` or `1.234e6`
- Ensure readability and precision

### 8.4 Precision and Rounding

- Default precision: 4-6 decimal places
- Round appropriately to avoid floating-point errors
- Display significant figures correctly
- Handle trailing zeros appropriately

### 8.5 Performance Optimization

- JSON files loaded at startup (cached in memory)
- No database queries needed
- Fast lookup using dictionaries/maps
- Minimal memory footprint

### 8.6 Extensibility

- Easy to add more units to existing categories
- Easy to add more categories in future phases
- Consistent structure across all categories
- Well-documented conversion factor sources

---

## 9. Notes

- All new categories must follow the same architecture patterns as existing categories
- Maintain backward compatibility - no breaking changes to existing API contracts
- All conversion factors should be verified against authoritative sources (NIST, BIPM)
- Consider adding unit descriptions/definitions in future enhancements
- Consider adding unit conversion history/favorites for new categories
- Consider adding unit conversion formulas display for educational purposes

---

**Document Version**: 1.0  
**Last Updated**: [Current Date]  
**Status**: Draft for Review

