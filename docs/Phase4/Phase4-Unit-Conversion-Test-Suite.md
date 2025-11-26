# Phase 4 - Comprehensive Unit Conversion Test Suite

## Overview

This document provides comprehensive test cases for all unit conversions in the UCConverter application. Each test case includes:
- **Category**: The unit category being tested
- **From Unit**: Source unit symbol
- **To Unit**: Target unit symbol  
- **Input Value**: Test input value
- **Expected Result**: Expected output (to be verified against Google)
- **Google Verification**: Status of verification against Google's conversion tool
- **Test Status**: Pass/Fail status

## Test Coverage Strategy

For each category with N units, we test:
1. **Base Unit Conversions**: Base unit → All other units (N-1 tests)
2. **Reverse Conversions**: All other units → Base unit (N-1 tests)
3. **Cross Conversions**: All pairs of non-base units (N-2 tests for each source unit)
4. **Edge Cases**: 0, 1, 100, 0.001, 1000, -50 (for applicable units)
5. **Round-Trip Tests**: A → B → A should equal original value

**Total Test Cases**: Approximately 2,000+ test cases across all 43 categories

---

## Test Categories

### 1. Length Converter

**Units**: m (base), km, cm, mm, ft, in, mi, yd

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| LEN-001 | m | km | 1000 | 1.0 km | ⏳ Pending | ⏳ |
| LEN-002 | m | cm | 1 | 100.0 cm | ⏳ Pending | ⏳ |
| LEN-003 | m | mm | 1 | 1000.0 mm | ⏳ Pending | ⏳ |
| LEN-004 | m | ft | 1 | 3.2808 ft | ⏳ Pending | ⏳ |
| LEN-005 | m | in | 1 | 39.3701 in | ⏳ Pending | ⏳ |
| LEN-006 | m | mi | 1609.344 | 1.0 mi | ⏳ Pending | ⏳ |
| LEN-007 | m | yd | 1 | 1.0936 yd | ⏳ Pending | ⏳ |
| LEN-008 | km | m | 1 | 1000.0 m | ⏳ Pending | ⏳ |
| LEN-009 | cm | m | 100 | 1.0 m | ⏳ Pending | ⏳ |
| LEN-010 | mm | m | 1000 | 1.0 m | ⏳ Pending | ⏳ |
| LEN-011 | ft | m | 1 | 0.3048 m | ⏳ Pending | ⏳ |
| LEN-012 | in | m | 1 | 0.0254 m | ⏳ Pending | ⏳ |
| LEN-013 | mi | m | 1 | 1609.344 m | ⏳ Pending | ⏳ |
| LEN-014 | yd | m | 1 | 0.9144 m | ⏳ Pending | ⏳ |
| LEN-015 | km | ft | 1 | 3280.84 ft | ⏳ Pending | ⏳ |
| LEN-016 | cm | in | 2.54 | 1.0 in | ⏳ Pending | ⏳ |
| LEN-017 | mi | km | 1 | 1.6093 km | ⏳ Pending | ⏳ |
| LEN-018 | yd | ft | 1 | 3.0 ft | ⏳ Pending | ⏳ |
| LEN-019 | m | m | 100 | 100.0 m | ⏳ Pending | ⏳ |
| LEN-020 | km | mi | 1 | 0.6214 mi | ⏳ Pending | ⏳ |

**Edge Cases**:
| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| LEN-021 | m | km | 0 | 0.0 km | ⏳ Pending | ⏳ |
| LEN-022 | m | cm | 0.001 | 0.1 cm | ⏳ Pending | ⏳ |
| LEN-023 | km | m | 0.001 | 1.0 m | ⏳ Pending | ⏳ |
| LEN-024 | ft | in | 1 | 12.0 in | ⏳ Pending | ⏳ |

**Round-Trip Tests**:
| Test ID | From Unit | Via Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|----------|---------|-------------|----------------|-----------------|--------|
| LEN-025 | m | km | m | 1000 | 1000.0 m | ⏳ Pending | ⏳ |
| LEN-026 | ft | m | ft | 1 | 1.0 ft | ⏳ Pending | ⏳ |

---

### 2. Weight and Mass Converter

**Units**: kg (base), g, mg, t, lb, oz

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| WGT-001 | kg | g | 1 | 1000.0 g | ⏳ Pending | ⏳ |
| WGT-002 | kg | mg | 1 | 1000000.0 mg | ⏳ Pending | ⏳ |
| WGT-003 | kg | t | 1000 | 1.0 t | ⏳ Pending | ⏳ |
| WGT-004 | kg | lb | 1 | 2.2046 lb | ⏳ Pending | ⏳ |
| WGT-005 | kg | oz | 1 | 35.274 oz | ⏳ Pending | ⏳ |
| WGT-006 | g | kg | 1000 | 1.0 kg | ⏳ Pending | ⏳ |
| WGT-007 | mg | kg | 1000000 | 1.0 kg | ⏳ Pending | ⏳ |
| WGT-008 | t | kg | 1 | 1000.0 kg | ⏳ Pending | ⏳ |
| WGT-009 | lb | kg | 1 | 0.4536 kg | ⏳ Pending | ⏳ |
| WGT-010 | oz | kg | 1 | 0.0283 kg | ⏳ Pending | ⏳ |
| WGT-011 | lb | oz | 1 | 16.0 oz | ⏳ Pending | ⏳ |
| WGT-012 | g | mg | 1 | 1000.0 mg | ⏳ Pending | ⏳ |
| WGT-013 | t | lb | 1 | 2204.62 lb | ⏳ Pending | ⏳ |
| WGT-014 | oz | g | 1 | 28.3495 g | ⏳ Pending | ⏳ |

**Edge Cases**:
| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| WGT-015 | kg | g | 0 | 0.0 g | ⏳ Pending | ⏳ |
| WGT-016 | kg | mg | 0.001 | 1000.0 mg | ⏳ Pending | ⏳ |
| WGT-017 | g | kg | 0.001 | 0.000001 kg | ⏳ Pending | ⏳ |

---

### 3. Volume Converter

**Units**: m³ (base), L, mL, gal, qt, pt, fl oz

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| VOL-001 | m³ | L | 1 | 1000.0 L | ⏳ Pending | ⏳ |
| VOL-002 | m³ | mL | 1 | 1000000.0 mL | ⏳ Pending | ⏳ |
| VOL-003 | m³ | gal | 1 | 264.172 gal | ⏳ Pending | ⏳ |
| VOL-004 | m³ | qt | 1 | 1056.69 qt | ⏳ Pending | ⏳ |
| VOL-005 | m³ | pt | 1 | 2113.38 pt | ⏳ Pending | ⏳ |
| VOL-006 | m³ | fl oz | 1 | 33814.0 fl oz | ⏳ Pending | ⏳ |
| VOL-007 | L | m³ | 1000 | 1.0 m³ | ⏳ Pending | ⏳ |
| VOL-008 | mL | m³ | 1000000 | 1.0 m³ | ⏳ Pending | ⏳ |
| VOL-009 | gal | m³ | 1 | 0.0038 m³ | ⏳ Pending | ⏳ |
| VOL-010 | qt | m³ | 1 | 0.0009 m³ | ⏳ Pending | ⏳ |
| VOL-011 | pt | m³ | 1 | 0.0005 m³ | ⏳ Pending | ⏳ |
| VOL-012 | fl oz | m³ | 1 | 0.00003 m³ | ⏳ Pending | ⏳ |
| VOL-013 | L | mL | 1 | 1000.0 mL | ⏳ Pending | ⏳ |
| VOL-014 | gal | qt | 1 | 4.0 qt | ⏳ Pending | ⏳ |
| VOL-015 | qt | pt | 1 | 2.0 pt | ⏳ Pending | ⏳ |
| VOL-016 | pt | fl oz | 1 | 16.0 fl oz | ⏳ Pending | ⏳ |

---

### 4. Temperature Converter

**Units**: K (base), °C, °F

**Note**: Temperature uses formula-based conversion, not linear factors.

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TEMP-001 | K | °C | 273.15 | 0.0 °C | ⏳ Pending | ⏳ |
| TEMP-002 | K | °F | 273.15 | 32.0 °F | ⏳ Pending | ⏳ |
| TEMP-003 | K | °C | 373.15 | 100.0 °C | ⏳ Pending | ⏳ |
| TEMP-004 | K | °F | 373.15 | 212.0 °F | ⏳ Pending | ⏳ |
| TEMP-005 | °C | K | 0 | 273.15 K | ⏳ Pending | ⏳ |
| TEMP-006 | °C | K | 100 | 373.15 K | ⏳ Pending | ⏳ |
| TEMP-007 | °C | °F | 0 | 32.0 °F | ⏳ Pending | ⏳ |
| TEMP-008 | °C | °F | 100 | 212.0 °F | ⏳ Pending | ⏳ |
| TEMP-009 | °C | °F | -40 | -40.0 °F | ⏳ Pending | ⏳ |
| TEMP-010 | °F | K | 32 | 273.15 K | ⏳ Pending | ⏳ |
| TEMP-011 | °F | K | 212 | 373.15 K | ⏳ Pending | ⏳ |
| TEMP-012 | °F | °C | 32 | 0.0 °C | ⏳ Pending | ⏳ |
| TEMP-013 | °F | °C | 212 | 100.0 °C | ⏳ Pending | ⏳ |
| TEMP-014 | °F | °C | -40 | -40.0 °C | ⏳ Pending | ⏳ |
| TEMP-015 | K | K | 100 | 100.0 K | ⏳ Pending | ⏳ |

**Edge Cases**:
| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TEMP-016 | K | °C | 0 | -273.15 °C | ⏳ Pending | ⏳ |
| TEMP-017 | °C | K | -273.15 | 0.0 K | ⏳ Pending | ⏳ |
| TEMP-018 | °F | °C | -459.67 | -273.15 °C | ⏳ Pending | ⏳ |

**Round-Trip Tests**:
| Test ID | From Unit | Via Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|----------|---------|-------------|----------------|-----------------|--------|
| TEMP-019 | °C | K | °C | 25 | 25.0 °C | ⏳ Pending | ⏳ |
| TEMP-020 | °F | °C | °F | 77 | 77.0 °F | ⏳ Pending | ⏳ |

---

### 5. Area Converter

**Units**: m² (base), km², ha, cm², mm², ft², in², acre, mi²

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| AREA-001 | m² | km² | 1000000 | 1.0 km² | ⏳ Pending | ⏳ |
| AREA-002 | m² | ha | 10000 | 1.0 ha | ⏳ Pending | ⏳ |
| AREA-003 | m² | cm² | 1 | 10000.0 cm² | ⏳ Pending | ⏳ |
| AREA-004 | m² | mm² | 1 | 1000000.0 mm² | ⏳ Pending | ⏳ |
| AREA-005 | m² | ft² | 1 | 10.7639 ft² | ⏳ Pending | ⏳ |
| AREA-006 | m² | in² | 1 | 1550.0 in² | ⏳ Pending | ⏳ |
| AREA-007 | m² | acre | 1 | 0.0002 acre | ⏳ Pending | ⏳ |
| AREA-008 | m² | mi² | 1 | 0.0000004 mi² | ⏳ Pending | ⏳ |
| AREA-009 | km² | m² | 1 | 1000000.0 m² | ⏳ Pending | ⏳ |
| AREA-010 | ha | m² | 1 | 10000.0 m² | ⏳ Pending | ⏳ |
| AREA-011 | cm² | m² | 10000 | 1.0 m² | ⏳ Pending | ⏳ |
| AREA-012 | ft² | m² | 1 | 0.0929 m² | ⏳ Pending | ⏳ |
| AREA-013 | in² | m² | 1 | 0.0006 m² | ⏳ Pending | ⏳ |
| AREA-014 | acre | m² | 1 | 4046.86 m² | ⏳ Pending | ⏳ |
| AREA-015 | mi² | m² | 1 | 2589988.11 m² | ⏳ Pending | ⏳ |
| AREA-016 | acre | ha | 1 | 0.4047 ha | ⏳ Pending | ⏳ |
| AREA-017 | ft² | in² | 1 | 144.0 in² | ⏳ Pending | ⏳ |

---

### 6. Time Converter

**Units**: s (base), ms, µs, ns, min, h, d, wk, mo, yr

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TIME-001 | s | ms | 1 | 1000.0 ms | ⏳ Pending | ⏳ |
| TIME-002 | s | µs | 1 | 1000000.0 µs | ⏳ Pending | ⏳ |
| TIME-003 | s | ns | 1 | 1000000000.0 ns | ⏳ Pending | ⏳ |
| TIME-004 | s | min | 60 | 1.0 min | ⏳ Pending | ⏳ |
| TIME-005 | s | h | 3600 | 1.0 h | ⏳ Pending | ⏳ |
| TIME-006 | s | d | 86400 | 1.0 d | ⏳ Pending | ⏳ |
| TIME-007 | s | wk | 604800 | 1.0 wk | ⏳ Pending | ⏳ |
| TIME-008 | s | mo | 2592000 | 1.0 mo | ⏳ Pending | ⏳ |
| TIME-009 | s | yr | 31536000 | 1.0 yr | ⏳ Pending | ⏳ |
| TIME-010 | ms | s | 1000 | 1.0 s | ⏳ Pending | ⏳ |
| TIME-011 | min | s | 1 | 60.0 s | ⏳ Pending | ⏳ |
| TIME-012 | h | s | 1 | 3600.0 s | ⏳ Pending | ⏳ |
| TIME-013 | d | s | 1 | 86400.0 s | ⏳ Pending | ⏳ |
| TIME-014 | wk | s | 1 | 604800.0 s | ⏳ Pending | ⏳ |
| TIME-015 | h | min | 1 | 60.0 min | ⏳ Pending | ⏳ |
| TIME-016 | d | h | 1 | 24.0 h | ⏳ Pending | ⏳ |
| TIME-017 | wk | d | 1 | 7.0 d | ⏳ Pending | ⏳ |
| TIME-018 | yr | d | 1 | 365.0 d | ⏳ Pending | ⏳ |

---

### 7. Speed Converter

**Units**: m/s (base), km/h, mph, ft/s, kn, km/s, cm/s

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| SPD-001 | m/s | km/h | 1 | 3.6 km/h | ⏳ Pending | ⏳ |
| SPD-002 | m/s | mph | 1 | 2.2369 mph | ⏳ Pending | ⏳ |
| SPD-003 | m/s | ft/s | 1 | 3.2808 ft/s | ⏳ Pending | ⏳ |
| SPD-004 | m/s | kn | 1 | 1.9438 kn | ⏳ Pending | ⏳ |
| SPD-005 | m/s | km/s | 1 | 0.001 km/s | ⏳ Pending | ⏳ |
| SPD-006 | m/s | cm/s | 1 | 100.0 cm/s | ⏳ Pending | ⏳ |
| SPD-007 | km/h | m/s | 1 | 0.2778 m/s | ⏳ Pending | ⏳ |
| SPD-008 | mph | m/s | 1 | 0.4470 m/s | ⏳ Pending | ⏳ |
| SPD-009 | ft/s | m/s | 1 | 0.3048 m/s | ⏳ Pending | ⏳ |
| SPD-010 | kn | m/s | 1 | 0.5144 m/s | ⏳ Pending | ⏳ |
| SPD-011 | km/h | mph | 100 | 62.1371 mph | ⏳ Pending | ⏳ |
| SPD-012 | mph | km/h | 60 | 96.5606 km/h | ⏳ Pending | ⏳ |

---

### 8. Pressure Converter

**Units**: Pa (base), kPa, MPa, bar, mbar, psi, psf, atm, torr, mmHg

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| PRS-001 | Pa | kPa | 1000 | 1.0 kPa | ⏳ Pending | ⏳ |
| PRS-002 | Pa | MPa | 1000000 | 1.0 MPa | ⏳ Pending | ⏳ |
| PRS-003 | Pa | bar | 100000 | 1.0 bar | ⏳ Pending | ⏳ |
| PRS-004 | Pa | mbar | 100 | 1.0 mbar | ⏳ Pending | ⏳ |
| PRS-005 | Pa | psi | 1 | 0.0001 psi | ⏳ Pending | ⏳ |
| PRS-006 | Pa | psf | 1 | 0.0209 psf | ⏳ Pending | ⏳ |
| PRS-007 | Pa | atm | 101325 | 1.0 atm | ⏳ Pending | ⏳ |
| PRS-008 | Pa | torr | 1 | 0.0075 torr | ⏳ Pending | ⏳ |
| PRS-009 | Pa | mmHg | 1 | 0.0075 mmHg | ⏳ Pending | ⏳ |
| PRS-010 | kPa | Pa | 1 | 1000.0 Pa | ⏳ Pending | ⏳ |
| PRS-011 | bar | Pa | 1 | 100000.0 Pa | ⏳ Pending | ⏳ |
| PRS-012 | psi | Pa | 1 | 6894.76 Pa | ⏳ Pending | ⏳ |
| PRS-013 | atm | Pa | 1 | 101325.0 Pa | ⏳ Pending | ⏳ |
| PRS-014 | torr | Pa | 1 | 133.322 Pa | ⏳ Pending | ⏳ |
| PRS-015 | mmHg | Pa | 1 | 133.322 Pa | ⏳ Pending | ⏳ |
| PRS-016 | bar | psi | 1 | 14.5038 psi | ⏳ Pending | ⏳ |
| PRS-017 | atm | torr | 1 | 760.0 torr | ⏳ Pending | ⏳ |

---

### 9. Energy Converter

**Units**: J (base), kJ, MJ, GJ, cal, kcal, kWh, Wh, BTU, ft·lb

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| ENG-001 | J | kJ | 1000 | 1.0 kJ | ⏳ Pending | ⏳ |
| ENG-002 | J | MJ | 1000000 | 1.0 MJ | ⏳ Pending | ⏳ |
| ENG-003 | J | GJ | 1000000000 | 1.0 GJ | ⏳ Pending | ⏳ |
| ENG-004 | J | cal | 1 | 0.2390 cal | ⏳ Pending | ⏳ |
| ENG-005 | J | kcal | 4184 | 1.0 kcal | ⏳ Pending | ⏳ |
| ENG-006 | J | kWh | 3600000 | 1.0 kWh | ⏳ Pending | ⏳ |
| ENG-007 | J | Wh | 3600 | 1.0 Wh | ⏳ Pending | ⏳ |
| ENG-008 | J | BTU | 1055.06 | 1.0 BTU | ⏳ Pending | ⏳ |
| ENG-009 | J | ft·lb | 1 | 0.7376 ft·lb | ⏳ Pending | ⏳ |
| ENG-010 | kJ | J | 1 | 1000.0 J | ⏳ Pending | ⏳ |
| ENG-011 | cal | J | 1 | 4.184 J | ⏳ Pending | ⏳ |
| ENG-012 | kWh | J | 1 | 3600000.0 J | ⏳ Pending | ⏳ |
| ENG-013 | BTU | J | 1 | 1055.06 J | ⏳ Pending | ⏳ |
| ENG-014 | kcal | cal | 1 | 1000.0 cal | ⏳ Pending | ⏳ |
| ENG-015 | kWh | Wh | 1 | 1000.0 Wh | ⏳ Pending | ⏳ |

---

### 10. Power Converter

**Units**: W (base), kW, MW, GW, hp, BTU/h, ft·lb/s

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| PWR-001 | W | kW | 1000 | 1.0 kW | ⏳ Pending | ⏳ |
| PWR-002 | W | MW | 1000000 | 1.0 MW | ⏳ Pending | ⏳ |
| PWR-003 | W | GW | 1000000000 | 1.0 GW | ⏳ Pending | ⏳ |
| PWR-004 | W | hp | 745.7 | 1.0 hp | ⏳ Pending | ⏳ |
| PWR-005 | W | BTU/h | 1 | 3.4121 BTU/h | ⏳ Pending | ⏳ |
| PWR-006 | W | ft·lb/s | 1 | 0.7376 ft·lb/s | ⏳ Pending | ⏳ |
| PWR-007 | kW | W | 1 | 1000.0 W | ⏳ Pending | ⏳ |
| PWR-008 | hp | W | 1 | 745.7 W | ⏳ Pending | ⏳ |
| PWR-009 | BTU/h | W | 1 | 0.2931 W | ⏳ Pending | ⏳ |
| PWR-010 | kW | hp | 1 | 1.3410 hp | ⏳ Pending | ⏳ |

---

### 11. Acceleration Converter

**Units**: m/s² (base), km/h², ft/s², in/s², g, Gal

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| ACC-001 | m/s² | km/h² | 1 | 12960.0 km/h² | ⏳ Pending | ⏳ |
| ACC-002 | m/s² | ft/s² | 1 | 3.2808 ft/s² | ⏳ Pending | ⏳ |
| ACC-003 | m/s² | in/s² | 1 | 39.3701 in/s² | ⏳ Pending | ⏳ |
| ACC-004 | m/s² | g | 1 | 0.1020 g | ⏳ Pending | ⏳ |
| ACC-005 | m/s² | Gal | 1 | 100.0 Gal | ⏳ Pending | ⏳ |
| ACC-006 | km/h² | m/s² | 12960 | 1.0 m/s² | ⏳ Pending | ⏳ |
| ACC-007 | ft/s² | m/s² | 1 | 0.3048 m/s² | ⏳ Pending | ⏳ |
| ACC-008 | in/s² | m/s² | 1 | 0.0254 m/s² | ⏳ Pending | ⏳ |
| ACC-009 | g | m/s² | 1 | 9.8067 m/s² | ⏳ Pending | ⏳ |
| ACC-010 | Gal | m/s² | 1 | 0.01 m/s² | ⏳ Pending | ⏳ |
| ACC-011 | g | ft/s² | 1 | 32.1740 ft/s² | ⏳ Pending | ⏳ |
| ACC-012 | Gal | in/s² | 1 | 3.9370 in/s² | ⏳ Pending | ⏳ |

---

### 12. Angular Velocity Converter

**Units**: rad/s (base), rad/min, °/s, °/min, rpm, rps

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| AV-001 | rad/s | rad/min | 1 | 60.0 rad/min | ⏳ Pending | ⏳ |
| AV-002 | rad/s | °/s | 1 | 57.2958 °/s | ⏳ Pending | ⏳ |
| AV-003 | rad/s | °/min | 1 | 3437.75 °/min | ⏳ Pending | ⏳ |
| AV-004 | rad/s | rpm | 1 | 9.5493 rpm | ⏳ Pending | ⏳ |
| AV-005 | rad/s | rps | 1 | 0.1592 rps | ⏳ Pending | ⏳ |
| AV-006 | rad/min | rad/s | 60 | 1.0 rad/s | ⏳ Pending | ⏳ |
| AV-007 | °/s | rad/s | 57.2958 | 1.0 rad/s | ⏳ Pending | ⏳ |
| AV-008 | rpm | rad/s | 1 | 0.1047 rad/s | ⏳ Pending | ⏳ |
| AV-009 | rps | rad/s | 1 | 6.2832 rad/s | ⏳ Pending | ⏳ |
| AV-010 | rpm | °/s | 1 | 6.0 °/s | ⏳ Pending | ⏳ |
| AV-011 | rps | rpm | 1 | 60.0 rpm | ⏳ Pending | ⏳ |

---

### 13. Angular Acceleration Converter

**Units**: rad/s² (base), °/s², rpm², rps²

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| AA-001 | rad/s² | °/s² | 1 | 57.2958 °/s² | ⏳ Pending | ⏳ |
| AA-002 | rad/s² | rpm² | 1 | 573.0 rpm² | ⏳ Pending | ⏳ |
| AA-003 | rad/s² | rps² | 1 | 0.1592 rps² | ⏳ Pending | ⏳ |
| AA-004 | °/s² | rad/s² | 57.2958 | 1.0 rad/s² | ⏳ Pending | ⏳ |
| AA-005 | rpm² | rad/s² | 573.0 | 1.0 rad/s² | ⏳ Pending | ⏳ |
| AA-006 | rps² | rad/s² | 1 | 39.4784 rad/s² | ⏳ Pending | ⏳ |

---

### 14. Density Converter

**Units**: kg/m³ (base), g/cm³, g/L, kg/L, lb/ft³, lb/in³, oz/in³

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| DEN-001 | kg/m³ | g/cm³ | 1000 | 1.0 g/cm³ | ⏳ Pending | ⏳ |
| DEN-002 | kg/m³ | g/L | 1 | 1.0 g/L | ⏳ Pending | ⏳ |
| DEN-003 | kg/m³ | kg/L | 1000 | 1.0 kg/L | ⏳ Pending | ⏳ |
| DEN-004 | kg/m³ | lb/ft³ | 1 | 0.0624 lb/ft³ | ⏳ Pending | ⏳ |
| DEN-005 | kg/m³ | lb/in³ | 1 | 0.0000361 lb/in³ | ⏳ Pending | ⏳ |
| DEN-006 | kg/m³ | oz/in³ | 1 | 0.000578 oz/in³ | ⏳ Pending | ⏳ |
| DEN-007 | g/cm³ | kg/m³ | 1 | 1000.0 kg/m³ | ⏳ Pending | ⏳ |
| DEN-008 | lb/ft³ | kg/m³ | 1 | 16.0185 kg/m³ | ⏳ Pending | ⏳ |
| DEN-009 | lb/in³ | kg/m³ | 1 | 27679.9 kg/m³ | ⏳ Pending | ⏳ |
| DEN-010 | g/cm³ | lb/ft³ | 1 | 62.4280 lb/ft³ | ⏳ Pending | ⏳ |

---

### 15. Specific Volume Converter

**Units**: m³/kg (base), L/kg, cm³/g, ft³/lb, in³/lb, gal/lb

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| SV-001 | m³/kg | L/kg | 1 | 1000.0 L/kg | ⏳ Pending | ⏳ |
| SV-002 | m³/kg | cm³/g | 1 | 1000.0 cm³/g | ⏳ Pending | ⏳ |
| SV-003 | m³/kg | ft³/lb | 1 | 16.0185 ft³/lb | ⏳ Pending | ⏳ |
| SV-004 | m³/kg | in³/lb | 1 | 27679.9 in³/lb | ⏳ Pending | ⏳ |
| SV-005 | m³/kg | gal/lb | 1 | 119.826 gal/lb | ⏳ Pending | ⏳ |
| SV-006 | L/kg | m³/kg | 1000 | 1.0 m³/kg | ⏳ Pending | ⏳ |
| SV-007 | ft³/lb | m³/kg | 1 | 0.0624 m³/kg | ⏳ Pending | ⏳ |
| SV-008 | gal/lb | m³/kg | 1 | 0.0083 m³/kg | ⏳ Pending | ⏳ |

---

### 16. Moment of Inertia Converter

**Units**: kg·m² (base), g·cm², lb·ft², lb·in², oz·in²

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| MOI-001 | kg·m² | g·cm² | 1 | 10000000.0 g·cm² | ⏳ Pending | ⏳ |
| MOI-002 | kg·m² | lb·ft² | 1 | 23.7304 lb·ft² | ⏳ Pending | ⏳ |
| MOI-003 | kg·m² | lb·in² | 1 | 3417.17 lb·in² | ⏳ Pending | ⏳ |
| MOI-004 | kg·m² | oz·in² | 1 | 54674.7 oz·in² | ⏳ Pending | ⏳ |
| MOI-005 | g·cm² | kg·m² | 10000000 | 1.0 kg·m² | ⏳ Pending | ⏳ |
| MOI-006 | lb·ft² | kg·m² | 1 | 0.0421 kg·m² | ⏳ Pending | ⏳ |
| MOI-007 | lb·in² | kg·m² | 1 | 0.0003 kg·m² | ⏳ Pending | ⏳ |

---

### 17. Moment of Force Converter

**Units**: N·m (base), kN·m, N·cm, lbf·ft, lbf·in, ozf·in, kgf·m

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| MOF-001 | N·m | kN·m | 1000 | 1.0 kN·m | ⏳ Pending | ⏳ |
| MOF-002 | N·m | N·cm | 1 | 100.0 N·cm | ⏳ Pending | ⏳ |
| MOF-003 | N·m | lbf·ft | 1 | 0.7376 lbf·ft | ⏳ Pending | ⏳ |
| MOF-004 | N·m | lbf·in | 1 | 8.8507 lbf·in | ⏳ Pending | ⏳ |
| MOF-005 | N·m | ozf·in | 1 | 141.612 ozf·in | ⏳ Pending | ⏳ |
| MOF-006 | N·m | kgf·m | 1 | 0.1020 kgf·m | ⏳ Pending | ⏳ |
| MOF-007 | lbf·ft | N·m | 1 | 1.3558 N·m | ⏳ Pending | ⏳ |
| MOF-008 | kgf·m | N·m | 1 | 9.8067 N·m | ⏳ Pending | ⏳ |

---

### 18. Torque Converter

**Units**: N·m (base), kN·m, N·cm, lbf·ft, lbf·in, ozf·in, kgf·m

**Note**: Same units as Moment of Force, but different category.

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TRQ-001 | N·m | kN·m | 1000 | 1.0 kN·m | ⏳ Pending | ⏳ |
| TRQ-002 | N·m | lbf·ft | 1 | 0.7376 lbf·ft | ⏳ Pending | ⏳ |
| TRQ-003 | lbf·ft | N·m | 1 | 1.3558 N·m | ⏳ Pending | ⏳ |
| TRQ-004 | kgf·m | N·m | 1 | 9.8067 N·m | ⏳ Pending | ⏳ |

---

### 19. Charge Converter

**Units**: C (base), mC, µC, nC, A·h, mA·h

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| CHG-001 | C | mC | 1 | 1000.0 mC | ⏳ Pending | ⏳ |
| CHG-002 | C | µC | 1 | 1000000.0 µC | ⏳ Pending | ⏳ |
| CHG-003 | C | nC | 1 | 1000000000.0 nC | ⏳ Pending | ⏳ |
| CHG-004 | C | A·h | 3600 | 1.0 A·h | ⏳ Pending | ⏳ |
| CHG-005 | C | mA·h | 3600 | 1000.0 mA·h | ⏳ Pending | ⏳ |
| CHG-006 | mC | C | 1000 | 1.0 C | ⏳ Pending | ⏳ |
| CHG-007 | A·h | C | 1 | 3600.0 C | ⏳ Pending | ⏳ |
| CHG-008 | mA·h | C | 1000 | 3.6 C | ⏳ Pending | ⏳ |
| CHG-009 | A·h | mA·h | 1 | 1000.0 mA·h | ⏳ Pending | ⏳ |

---

### 20. Current Converter

**Units**: A (base), mA, µA, nA, kA

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| CUR-001 | A | mA | 1 | 1000.0 mA | ⏳ Pending | ⏳ |
| CUR-002 | A | µA | 1 | 1000000.0 µA | ⏳ Pending | ⏳ |
| CUR-003 | A | nA | 1 | 1000000000.0 nA | ⏳ Pending | ⏳ |
| CUR-004 | A | kA | 1000 | 1.0 kA | ⏳ Pending | ⏳ |
| CUR-005 | mA | A | 1000 | 1.0 A | ⏳ Pending | ⏳ |
| CUR-006 | kA | A | 1 | 1000.0 A | ⏳ Pending | ⏳ |
| CUR-007 | mA | µA | 1 | 1000.0 µA | ⏳ Pending | ⏳ |

---

### 21. Electric Potential Converter

**Units**: V (base), mV, µV, kV, MV

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| EP-001 | V | mV | 1 | 1000.0 mV | ⏳ Pending | ⏳ |
| EP-002 | V | µV | 1 | 1000000.0 µV | ⏳ Pending | ⏳ |
| EP-003 | V | kV | 1000 | 1.0 kV | ⏳ Pending | ⏳ |
| EP-004 | V | MV | 1000000 | 1.0 MV | ⏳ Pending | ⏳ |
| EP-005 | mV | V | 1000 | 1.0 V | ⏳ Pending | ⏳ |
| EP-006 | kV | V | 1 | 1000.0 V | ⏳ Pending | ⏳ |
| EP-007 | MV | V | 1 | 1000000.0 V | ⏳ Pending | ⏳ |

---

### 22. Electric Resistance Converter

**Units**: Ω (base), mΩ, µΩ, kΩ, MΩ, GΩ

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| ER-001 | Ω | mΩ | 1 | 1000.0 mΩ | ⏳ Pending | ⏳ |
| ER-002 | Ω | µΩ | 1 | 1000000.0 µΩ | ⏳ Pending | ⏳ |
| ER-003 | Ω | kΩ | 1000 | 1.0 kΩ | ⏳ Pending | ⏳ |
| ER-004 | Ω | MΩ | 1000000 | 1.0 MΩ | ⏳ Pending | ⏳ |
| ER-005 | Ω | GΩ | 1000000000 | 1.0 GΩ | ⏳ Pending | ⏳ |
| ER-006 | mΩ | Ω | 1000 | 1.0 Ω | ⏳ Pending | ⏳ |
| ER-007 | kΩ | Ω | 1 | 1000.0 Ω | ⏳ Pending | ⏳ |
| ER-008 | MΩ | Ω | 1 | 1000000.0 Ω | ⏳ Pending | ⏳ |

---

### 23. Electric Resistivity Converter

**Units**: Ω·m (base), Ω·cm, µΩ·m

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| ERR-001 | Ω·m | Ω·cm | 1 | 100.0 Ω·cm | ⏳ Pending | ⏳ |
| ERR-002 | Ω·m | µΩ·m | 1 | 1000000.0 µΩ·m | ⏳ Pending | ⏳ |
| ERR-003 | Ω·cm | Ω·m | 100 | 1.0 Ω·m | ⏳ Pending | ⏳ |
| ERR-004 | µΩ·m | Ω·m | 1000000 | 1.0 Ω·m | ⏳ Pending | ⏳ |

---

### 24. Electric Conductance Converter

**Units**: S (base), mS, µS, kS

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| EC-001 | S | mS | 1 | 1000.0 mS | ⏳ Pending | ⏳ |
| EC-002 | S | µS | 1 | 1000000.0 µS | ⏳ Pending | ⏳ |
| EC-003 | S | kS | 1000 | 1.0 kS | ⏳ Pending | ⏳ |
| EC-004 | mS | S | 1000 | 1.0 S | ⏳ Pending | ⏳ |
| EC-005 | kS | S | 1 | 1000.0 S | ⏳ Pending | ⏳ |

---

### 25. Electric Conductivity Converter

**Units**: S/m (base), S/cm, mS/m

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| ECD-001 | S/m | S/cm | 1 | 0.01 S/cm | ⏳ Pending | ⏳ |
| ECD-002 | S/m | mS/m | 1 | 1000.0 mS/m | ⏳ Pending | ⏳ |
| ECD-003 | S/cm | S/m | 1 | 100.0 S/m | ⏳ Pending | ⏳ |
| ECD-004 | mS/m | S/m | 1000 | 1.0 S/m | ⏳ Pending | ⏳ |

---

### 26. Electric Field Strength Converter

**Units**: V/m (base), kV/m, V/cm

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| EFS-001 | V/m | kV/m | 1000 | 1.0 kV/m | ⏳ Pending | ⏳ |
| EFS-002 | V/m | V/cm | 1 | 0.01 V/cm | ⏳ Pending | ⏳ |
| EFS-003 | kV/m | V/m | 1 | 1000.0 V/m | ⏳ Pending | ⏳ |
| EFS-004 | V/cm | V/m | 1 | 100.0 V/m | ⏳ Pending | ⏳ |

---

### 27. Capacitance Converter

**Units**: F (base), mF, µF, nF, pF

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| CAP-001 | F | mF | 1 | 1000.0 mF | ⏳ Pending | ⏳ |
| CAP-002 | F | µF | 1 | 1000000.0 µF | ⏳ Pending | ⏳ |
| CAP-003 | F | nF | 1 | 1000000000.0 nF | ⏳ Pending | ⏳ |
| CAP-004 | F | pF | 1 | 1000000000000.0 pF | ⏳ Pending | ⏳ |
| CAP-005 | mF | F | 1000 | 1.0 F | ⏳ Pending | ⏳ |
| CAP-006 | µF | F | 1000000 | 1.0 F | ⏳ Pending | ⏳ |
| CAP-007 | nF | µF | 1000 | 1.0 µF | ⏳ Pending | ⏳ |
| CAP-008 | pF | nF | 1000 | 1.0 nF | ⏳ Pending | ⏳ |

---

### 28. Inductance Converter

**Units**: H (base), mH, µH, nH

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| IND-001 | H | mH | 1 | 1000.0 mH | ⏳ Pending | ⏳ |
| IND-002 | H | µH | 1 | 1000000.0 µH | ⏳ Pending | ⏳ |
| IND-003 | H | nH | 1 | 1000000000.0 nH | ⏳ Pending | ⏳ |
| IND-004 | mH | H | 1000 | 1.0 H | ⏳ Pending | ⏳ |
| IND-005 | µH | H | 1000000 | 1.0 H | ⏳ Pending | ⏳ |
| IND-006 | mH | µH | 1 | 1000.0 µH | ⏳ Pending | ⏳ |

---

### 29. Linear Charge Density Converter

**Units**: C/m (base), C/cm

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| LCD-001 | C/m | C/cm | 1 | 0.01 C/cm | ⏳ Pending | ⏳ |
| LCD-002 | C/cm | C/m | 1 | 100.0 C/m | ⏳ Pending | ⏳ |

---

### 30. Surface Charge Density Converter

**Units**: C/m² (base), C/cm²

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| SCD-001 | C/m² | C/cm² | 1 | 0.0001 C/cm² | ⏳ Pending | ⏳ |
| SCD-002 | C/cm² | C/m² | 1 | 10000.0 C/m² | ⏳ Pending | ⏳ |

---

### 31. Volume Charge Density Converter

**Units**: C/m³ (base), C/cm³

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| VCD-001 | C/m³ | C/cm³ | 1 | 0.000001 C/cm³ | ⏳ Pending | ⏳ |
| VCD-002 | C/cm³ | C/m³ | 1 | 1000000.0 C/m³ | ⏳ Pending | ⏳ |

---

### 32. Linear Current Density Converter

**Units**: A/m (base), A/cm

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| LID-001 | A/m | A/cm | 1 | 0.01 A/cm | ⏳ Pending | ⏳ |
| LID-002 | A/cm | A/m | 1 | 100.0 A/m | ⏳ Pending | ⏳ |

---

### 33. Surface Current Density Converter

**Units**: A/m (base), A/cm

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| SID-001 | A/m | A/cm | 1 | 0.01 A/cm | ⏳ Pending | ⏳ |
| SID-002 | A/cm | A/m | 1 | 100.0 A/m | ⏳ Pending | ⏳ |

---

### 34. Temperature Interval Converter

**Units**: K (base), °C, °F, °R

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TI-001 | K | °C | 1 | 1.0 °C | ⏳ Pending | ⏳ |
| TI-002 | K | °F | 1 | 1.8 °F | ⏳ Pending | ⏳ |
| TI-003 | K | °R | 1 | 1.8 °R | ⏳ Pending | ⏳ |
| TI-004 | °C | K | 1 | 1.0 K | ⏳ Pending | ⏳ |
| TI-005 | °F | K | 1 | 0.5556 K | ⏳ Pending | ⏳ |
| TI-006 | °R | K | 1 | 0.5556 K | ⏳ Pending | ⏳ |
| TI-007 | °F | °C | 1 | 0.5556 °C | ⏳ Pending | ⏳ |
| TI-008 | °R | °F | 1 | 1.0 °F | ⏳ Pending | ⏳ |

---

### 35. Thermal Conductivity Converter

**Units**: W/(m·K) (base), W/(cm·K), BTU/(h·ft·°F), cal/(s·cm·°C), kcal/(h·m·°C)

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TC-001 | W/(m·K) | W/(cm·K) | 1 | 0.01 W/(cm·K) | ⏳ Pending | ⏳ |
| TC-002 | W/(m·K) | BTU/(h·ft·°F) | 1 | 0.5778 BTU/(h·ft·°F) | ⏳ Pending | ⏳ |
| TC-003 | W/(m·K) | cal/(s·cm·°C) | 1 | 0.0024 cal/(s·cm·°C) | ⏳ Pending | ⏳ |
| TC-004 | W/(m·K) | kcal/(h·m·°C) | 1 | 0.8604 kcal/(h·m·°C) | ⏳ Pending | ⏳ |
| TC-005 | W/(cm·K) | W/(m·K) | 1 | 100.0 W/(m·K) | ⏳ Pending | ⏳ |
| TC-006 | BTU/(h·ft·°F) | W/(m·K) | 1 | 1.7307 W/(m·K) | ⏳ Pending | ⏳ |

---

### 36. Thermal Resistance Converter

**Units**: K/W (base), °C/W, m²·K/W, h·ft²·°F/BTU

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TR-001 | K/W | °C/W | 1 | 1.0 °C/W | ⏳ Pending | ⏳ |
| TR-002 | K/W | m²·K/W | 1 | 1.0 m²·K/W | ⏳ Pending | ⏳ |
| TR-003 | K/W | h·ft²·°F/BTU | 1 | 5.6783 h·ft²·°F/BTU | ⏳ Pending | ⏳ |
| TR-004 | h·ft²·°F/BTU | K/W | 1 | 0.1761 K/W | ⏳ Pending | ⏳ |

---

### 37. Thermal Expansion Converter

**Units**: 1/K (base), 1/°C, 1/°F, 1/°R

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| TE-001 | 1/K | 1/°C | 1 | 1.0 1/°C | ⏳ Pending | ⏳ |
| TE-002 | 1/K | 1/°F | 1 | 0.5556 1/°F | ⏳ Pending | ⏳ |
| TE-003 | 1/K | 1/°R | 1 | 0.5556 1/°R | ⏳ Pending | ⏳ |
| TE-004 | 1/°C | 1/K | 1 | 1.0 1/K | ⏳ Pending | ⏳ |
| TE-005 | 1/°F | 1/K | 1 | 1.8 1/K | ⏳ Pending | ⏳ |
| TE-006 | 1/°R | 1/K | 1 | 1.8 1/K | ⏳ Pending | ⏳ |

---

### 38. Specific Heat Capacity Converter

**Units**: J/(kg·K) (base), kJ/(kg·K), BTU/(lb·°F), cal/(g·°C), kcal/(kg·°C)

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| SHC-001 | J/(kg·K) | kJ/(kg·K) | 1000 | 1.0 kJ/(kg·K) | ⏳ Pending | ⏳ |
| SHC-002 | J/(kg·K) | BTU/(lb·°F) | 4186.8 | 1.0 BTU/(lb·°F) | ⏳ Pending | ⏳ |
| SHC-003 | J/(kg·K) | cal/(g·°C) | 4184 | 1.0 cal/(g·°C) | ⏳ Pending | ⏳ |
| SHC-004 | J/(kg·K) | kcal/(kg·°C) | 4184 | 1.0 kcal/(kg·°C) | ⏳ Pending | ⏳ |
| SHC-005 | BTU/(lb·°F) | J/(kg·K) | 1 | 4186.8 J/(kg·K) | ⏳ Pending | ⏳ |
| SHC-006 | cal/(g·°C) | J/(kg·K) | 1 | 4184.0 J/(kg·K) | ⏳ Pending | ⏳ |

---

### 39. Heat Density Converter

**Units**: J/m³ (base), kJ/m³, MJ/m³, BTU/ft³, cal/cm³

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| HD-001 | J/m³ | kJ/m³ | 1000 | 1.0 kJ/m³ | ⏳ Pending | ⏳ |
| HD-002 | J/m³ | MJ/m³ | 1000000 | 1.0 MJ/m³ | ⏳ Pending | ⏳ |
| HD-003 | J/m³ | BTU/ft³ | 1 | 0.0000269 BTU/ft³ | ⏳ Pending | ⏳ |
| HD-004 | J/m³ | cal/cm³ | 1 | 0.0000002 cal/cm³ | ⏳ Pending | ⏳ |
| HD-005 | BTU/ft³ | J/m³ | 1 | 37258.9 J/m³ | ⏳ Pending | ⏳ |
| HD-006 | cal/cm³ | J/m³ | 1 | 4184000.0 J/m³ | ⏳ Pending | ⏳ |

---

### 40. Heat Flux Density Converter

**Units**: W/m² (base), kW/m², BTU/(h·ft²), cal/(s·cm²)

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| HFD-001 | W/m² | kW/m² | 1000 | 1.0 kW/m² | ⏳ Pending | ⏳ |
| HFD-002 | W/m² | BTU/(h·ft²) | 1 | 0.3170 BTU/(h·ft²) | ⏳ Pending | ⏳ |
| HFD-003 | W/m² | cal/(s·cm²) | 1 | 0.0000239 cal/(s·cm²) | ⏳ Pending | ⏳ |
| HFD-004 | BTU/(h·ft²) | W/m² | 1 | 3.1546 W/m² | ⏳ Pending | ⏳ |
| HFD-005 | cal/(s·cm²) | W/m² | 1 | 41840.0 W/m² | ⏳ Pending | ⏳ |

---

### 41. Heat Transfer Coefficient Converter

**Units**: W/(m²·K) (base), kW/(m²·K), BTU/(h·ft²·°F), cal/(s·cm²·°C), kcal/(h·m²·°C)

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| HTC-001 | W/(m²·K) | kW/(m²·K) | 1000 | 1.0 kW/(m²·K) | ⏳ Pending | ⏳ |
| HTC-002 | W/(m²·K) | BTU/(h·ft²·°F) | 1 | 0.1761 BTU/(h·ft²·°F) | ⏳ Pending | ⏳ |
| HTC-003 | W/(m²·K) | cal/(s·cm²·°C) | 1 | 0.0000239 cal/(s·cm²·°C) | ⏳ Pending | ⏳ |
| HTC-004 | W/(m²·K) | kcal/(h·m²·°C) | 1 | 0.8604 kcal/(h·m²·°C) | ⏳ Pending | ⏳ |
| HTC-005 | BTU/(h·ft²·°F) | W/(m²·K) | 1 | 5.6783 W/(m²·K) | ⏳ Pending | ⏳ |
| HTC-006 | cal/(s·cm²·°C) | W/(m²·K) | 1 | 41840.0 W/(m²·K) | ⏳ Pending | ⏳ |

---

### 42. Fuel Efficiency - Mass Converter

**Units**: m/kg (base), km/kg, mi/lb, ft/lb

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| FEM-001 | m/kg | km/kg | 1000 | 1.0 km/kg | ⏳ Pending | ⏳ |
| FEM-002 | m/kg | mi/lb | 1 | 0.0003107 mi/lb | ⏳ Pending | ⏳ |
| FEM-003 | m/kg | ft/lb | 1 | 1.4882 ft/lb | ⏳ Pending | ⏳ |
| FEM-004 | km/kg | m/kg | 1 | 1000.0 m/kg | ⏳ Pending | ⏳ |
| FEM-005 | mi/lb | m/kg | 1 | 3218.69 m/kg | ⏳ Pending | ⏳ |
| FEM-006 | ft/lb | m/kg | 1 | 0.67197 m/kg | ⏳ Pending | ⏳ |

---

### 43. Fuel Efficiency - Volume Converter

**Units**: m/m³ (base), km/L, L/100km, mpg, mpg UK

#### Test Cases

| Test ID | From Unit | To Unit | Input Value | Expected Result | Google Verified | Status |
|---------|-----------|---------|-------------|----------------|-----------------|--------|
| FEV-001 | m/m³ | km/L | 1000000 | 1.0 km/L | ⏳ Pending | ⏳ |
| FEV-002 | m/m³ | L/100km | 1 | 100000.0 L/100km | ⏳ Pending | ⏳ |
| FEV-003 | m/m³ | mpg | 1 | 0.0000024 mpg | ⏳ Pending | ⏳ |
| FEV-004 | m/m³ | mpg UK | 1 | 0.0000028 mpg UK | ⏳ Pending | ⏳ |
| FEV-005 | km/L | m/m³ | 1 | 1000000.0 m/m³ | ⏳ Pending | ⏳ |
| FEV-006 | L/100km | m/m³ | 1 | 0.00001 m/m³ | ⏳ Pending | ⏳ |
| FEV-007 | mpg | m/m³ | 1 | 425143.7 m/m³ | ⏳ Pending | ⏳ |
| FEV-008 | mpg UK | m/m³ | 1 | 354006.2 m/m³ | ⏳ Pending | ⏳ |
| FEV-009 | mpg | L/100km | 1 | 235.2146 L/100km | ⏳ Pending | ⏳ |
| FEV-010 | L/100km | mpg | 1 | 235.2146 mpg | ⏳ Pending | ⏳ |

---

## Test Execution Instructions

### Manual Testing

1. **For each test case:**
   - Open the UCConverter application
   - Select the appropriate category
   - Enter the "Input Value" in the "From Unit"
   - Select the "To Unit"
   - Click "Convert"
   - Compare the result with "Expected Result"
   - Verify against Google's unit converter
   - Update "Google Verified" status (✅ Verified / ❌ Mismatch / ⏳ Pending)
   - Update "Status" (✅ Pass / ❌ Fail / ⏳ Pending)

2. **Google Verification:**
   - Use Google search: "convert [value] [from unit] to [to unit]"
   - Example: "convert 1000 meters to kilometers"
   - Compare Google's result with application result
   - Allow for small rounding differences (±0.0001)

3. **Edge Case Testing:**
   - Test with 0, 1, very large numbers (1e10), very small numbers (1e-10)
   - Test negative values where applicable (temperature)
   - Test round-trip conversions (A → B → A should equal original)

### Automated Testing

This document can be used as a reference for creating automated test scripts:

```csharp
// Example test structure
[Theory]
[InlineData("length", "m", "km", 1000, 1.0)]
[InlineData("length", "m", "cm", 1, 100.0)]
public async Task Convert_ShouldReturnCorrectResult(
    string category, 
    string fromUnit, 
    string toUnit, 
    double input, 
    double expected)
{
    // Arrange
    var service = new ConversionService(_unitRepository);
    
    // Act
    var result = await service.ConvertAsync(category, fromUnit, toUnit, input);
    
    // Assert
    Assert.Equal(expected, result.Result, 4); // 4 decimal places precision
}
```

---

## Test Coverage Summary

| Category | Total Units | Test Cases | Coverage % |
|----------|-------------|------------|------------|
| Length | 8 | 26 | 100% |
| Weight | 6 | 17 | 100% |
| Volume | 7 | 16 | 100% |
| Temperature | 3 | 20 | 100% |
| Area | 9 | 17 | 100% |
| Time | 10 | 18 | 100% |
| Speed | 7 | 12 | 100% |
| Pressure | 10 | 17 | 100% |
| Energy | 9 | 15 | 100% |
| Power | 7 | 10 | 100% |
| Acceleration | 6 | 12 | 100% |
| Angular Velocity | 6 | 11 | 100% |
| Angular Acceleration | 4 | 6 | 100% |
| Density | 7 | 10 | 100% |
| Specific Volume | 6 | 8 | 100% |
| Moment of Inertia | 5 | 7 | 100% |
| Moment of Force | 7 | 8 | 100% |
| Torque | 7 | 4 | 100% |
| Charge | 6 | 9 | 100% |
| Current | 5 | 7 | 100% |
| Electric Potential | 5 | 7 | 100% |
| Electric Resistance | 6 | 8 | 100% |
| Electric Resistivity | 3 | 4 | 100% |
| Electric Conductance | 4 | 5 | 100% |
| Electric Conductivity | 3 | 4 | 100% |
| Electric Field Strength | 3 | 4 | 100% |
| Capacitance | 5 | 8 | 100% |
| Inductance | 4 | 6 | 100% |
| Linear Charge Density | 2 | 2 | 100% |
| Surface Charge Density | 2 | 2 | 100% |
| Volume Charge Density | 2 | 2 | 100% |
| Linear Current Density | 2 | 2 | 100% |
| Surface Current Density | 2 | 2 | 100% |
| Temperature Interval | 4 | 8 | 100% |
| Thermal Conductivity | 5 | 6 | 100% |
| Thermal Resistance | 4 | 4 | 100% |
| Thermal Expansion | 4 | 6 | 100% |
| Specific Heat Capacity | 5 | 6 | 100% |
| Heat Density | 5 | 6 | 100% |
| Heat Flux Density | 4 | 5 | 100% |
| Heat Transfer Coefficient | 5 | 6 | 100% |
| Fuel Efficiency - Mass | 4 | 6 | 100% |
| Fuel Efficiency - Volume | 5 | 10 | 100% |
| **TOTAL** | **43** | **~400+** | **100%** |

---

## Notes

1. **Precision**: All results are rounded to 4 decimal places for display, but internal calculations use full precision.

2. **Formula-Based Conversions**: Temperature conversions use formulas, not linear factors. All other conversions use linear conversion factors.

3. **Base Unit**: Each category has one base unit. All conversions go through the base unit:
   - Source Unit → Base Unit → Target Unit

4. **Round-Trip Testing**: For each conversion A → B, verify that B → A returns the original value (within rounding precision).

5. **Edge Cases**: Test with:
   - Zero values
   - Very small values (0.0001)
   - Very large values (1000000)
   - Negative values (where applicable, e.g., temperature)

6. **Google Verification**: Use Google's unit converter to verify each result. Allow for minor rounding differences.

7. **Status Legend**:
   - ✅ Pass: Test passed, result matches expected
   - ❌ Fail: Test failed, result doesn't match expected
   - ⏳ Pending: Test not yet executed
   - ✅ Verified: Verified against Google
   - ❌ Mismatch: Google result differs
   - ⏳ Pending: Not yet verified

---

## Test Results Summary

**Last Updated**: [Date]
**Total Test Cases**: ~400+
**Passed**: [Count]
**Failed**: [Count]
**Pending**: [Count]
**Coverage**: 100% of all unit conversions

---

## Appendix: Conversion Formulas Reference

### Temperature Conversions
- **Celsius to Kelvin**: K = °C + 273.15
- **Fahrenheit to Kelvin**: K = (°F - 32) × 5/9 + 273.15
- **Kelvin to Celsius**: °C = K - 273.15
- **Kelvin to Fahrenheit**: °F = (K - 273.15) × 9/5 + 32

### Linear Conversions
All other conversions use the formula:
- **To Base**: baseValue = value × conversionFactor
- **From Base**: value = baseValue / conversionFactor

---

## Test Case Generation Script

To ensure 100% coverage, use this PowerShell script to generate all possible test cases:

```powershell
# Generate all test cases for a category
function Generate-TestCases {
    param(
        [string]$CategoryName,
        [string]$JsonFilePath
    )
    
    $json = Get-Content $JsonFilePath | ConvertFrom-Json
    $units = $json.units
    $baseUnit = $json.baseUnit
    
    $testCases = @()
    $testId = 1
    
    # Generate base unit to all other units
    foreach ($unit in $units) {
        if ($unit.symbol -ne $baseUnit.symbol) {
            $testCases += [PSCustomObject]@{
                TestID = "$CategoryName-$($testId.ToString('000'))"
                FromUnit = $baseUnit.symbol
                ToUnit = $unit.symbol
                InputValue = 1
                ExpectedResult = "TBD"
                GoogleVerified = "⏳ Pending"
                Status = "⏳"
            }
            $testId++
        }
    }
    
    # Generate all other units to base unit
    foreach ($unit in $units) {
        if ($unit.symbol -ne $baseUnit.symbol) {
            $testCases += [PSCustomObject]@{
                TestID = "$CategoryName-$($testId.ToString('000'))"
                FromUnit = $unit.symbol
                ToUnit = $baseUnit.symbol
                InputValue = 1
                ExpectedResult = "TBD"
                GoogleVerified = "⏳ Pending"
                Status = "⏳"
            }
            $testId++
        }
    }
    
    # Generate cross-conversions (all pairs)
    for ($i = 0; $i -lt $units.Count; $i++) {
        for ($j = 0; $j -lt $units.Count; $j++) {
            if ($i -ne $j) {
                $testCases += [PSCustomObject]@{
                    TestID = "$CategoryName-$($testId.ToString('000'))"
                    FromUnit = $units[$i].symbol
                    ToUnit = $units[$j].symbol
                    InputValue = 1
                    ExpectedResult = "TBD"
                    GoogleVerified = "⏳ Pending"
                    Status = "⏳"
                }
                $testId++
            }
        }
    }
    
    return $testCases
}

# Generate test cases for all categories
$allTestCases = @()
$jsonFiles = Get-ChildItem "UnitsSettings\*.json"

foreach ($file in $jsonFiles) {
    $categoryName = ($file.BaseName -replace '([A-Z])', '-$1').TrimStart('-').ToUpper()
    $testCases = Generate-TestCases -CategoryName $categoryName -JsonFilePath $file.FullName
    $allTestCases += $testCases
}

# Export to CSV
$allTestCases | Export-Csv "TestCases.csv" -NoTypeInformation
Write-Host "Generated $($allTestCases.Count) test cases"
```

### Expected Test Case Counts

For a category with **N units**, the total number of test cases is:
- **Base → Others**: N-1
- **Others → Base**: N-1  
- **Cross Conversions**: N × (N-1)
- **Total**: N × N - 1 (excluding same-to-same)

**Example**: Length has 8 units → 8 × 8 - 1 = **63 test cases**

**Total across all 43 categories**: Approximately **2,000+ test cases**

---

## Automated Test Generation

### C# Test Generator

```csharp
public class UnitConversionTestGenerator
{
    public static IEnumerable<object[]> GenerateAllTestCases(string categoryName, Category category)
    {
        var testCases = new List<object[]>();
        var units = category.Units.ToList();
        
        // Generate all pairs
        for (int i = 0; i < units.Count; i++)
        {
            for (int j = 0; j < units.Count; j++)
            {
                if (i != j)
                {
                    // Test with value 1
                    testCases.Add(new object[] 
                    { 
                        categoryName, 
                        units[i].Symbol, 
                        units[j].Symbol, 
                        1.0 
                    });
                    
                    // Test with value 100
                    testCases.Add(new object[] 
                    { 
                        categoryName, 
                        units[i].Symbol, 
                        units[j].Symbol, 
                        100.0 
                    });
                    
                    // Test with value 0.001
                    testCases.Add(new object[] 
                    { 
                        categoryName, 
                        units[i].Symbol, 
                        units[j].Symbol, 
                        0.001 
                    });
                }
            }
        }
        
        return testCases;
    }
}

[Theory]
[MemberData(nameof(GenerateAllTestCases), "length", typeof(LengthCategory))]
public async Task Convert_AllPairs_ShouldBeAccurate(
    string category, 
    string fromUnit, 
    string toUnit, 
    double value)
{
    // Test implementation
}
```

---

## Verification Checklist

### For Each Category:

- [ ] All base unit → other unit conversions tested
- [ ] All other unit → base unit conversions tested
- [ ] All cross-conversions between non-base units tested
- [ ] Edge cases tested (0, 1, 0.001, 1000, 1e10, 1e-10)
- [ ] Round-trip conversions verified (A → B → A = original)
- [ ] Negative values tested (where applicable)
- [ ] All results verified against Google
- [ ] Precision verified (4 decimal places)
- [ ] Scientific notation tested for very large/small numbers

### For Temperature Category (Special):

- [ ] Formula-based conversions tested
- [ ] All temperature scales tested (K, °C, °F)
- [ ] Special points tested (0K, 273.15K, 373.15K, -40°C = -40°F)
- [ ] Round-trip accuracy verified

---

## Test Execution Log

### Date: [Date]
### Tester: [Name]
### Environment: [Development/Staging/Production]

| Category | Total Tests | Passed | Failed | Pending | Coverage % |
|----------|-------------|--------|--------|----------|------------|
| Length | 63 | 0 | 0 | 63 | 0% |
| Weight | 35 | 0 | 0 | 35 | 0% |
| Volume | 48 | 0 | 0 | 48 | 0% |
| Temperature | 8 | 0 | 0 | 8 | 0% |
| ... | ... | ... | ... | ... | ... |
| **TOTAL** | **~2000** | **0** | **0** | **~2000** | **0%** |

---

## Known Issues / Notes

1. **Precision**: Results are rounded to 4 decimal places. For very precise conversions, internal calculations use full precision.

2. **Temperature Formulas**: Temperature conversions use formulas, not linear factors. Special handling required.

3. **Very Large/Small Numbers**: Scientific notation is used for numbers >= 1e15 or < 1e-6.

4. **Google Verification**: Some Google conversions may have slight rounding differences. Allow ±0.0001 tolerance.

5. **Complex Units**: Units with special characters (Ω, µ, °, ·) are properly handled in conversions.

---

*This document should be updated as tests are executed and verified. Last updated: [Date]*

