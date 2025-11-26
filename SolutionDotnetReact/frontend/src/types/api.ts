// API DTOs matching the backend structure

export interface UnitDto {
  symbol: string;
  name: string;
  displayName: string;
  isBaseUnit: boolean;
  isSIUnit: boolean;
  unitSystem: string;
}

export interface CategoryDto {
  name: string;
  displayName: string;
  group: string;
  baseUnit?: UnitDto;
  units?: UnitDto[];
}

export interface ConvertRequestDto {
  value: number;
  fromUnit: string;
  toUnit: string;
  category: string;
  locale?: string;
}

export interface ConvertResponseDto {
  value: number;
  fromUnit: string;
  toUnit: string;
  category: string;
  result: number;
  formula?: string;
}

export interface UnitInfoDto {
  symbol: string;
  name: string;
  category: string;
  isBaseUnit: boolean;
  isSIUnit: boolean;
  unitSystem: string;
}

