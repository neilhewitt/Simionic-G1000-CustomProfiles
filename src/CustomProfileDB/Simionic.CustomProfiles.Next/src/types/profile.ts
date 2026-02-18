import { AircraftType } from "./enums";
import { Gauge } from "./gauge";

export interface OwnerInfo {
  Id: string | null;
  Name: string | null;
}

export interface SettingRange {
  Min: number;
  Max: number;
}

export interface VacuumPSIRange {
  Min: number;
  Max: number;
  GreenStart: number;
  GreenEnd: number;
}

export interface FlapsRange {
  Markings: (string | null)[];
  Positions: (number | null)[];
}

export interface VSpeeds {
  Vs0: number;
  Vs1: number;
  Vfe: number;
  Vno: number;
  Vne: number;
  Vglide: number;
  Vr: number;
  Vx: number;
  Vy: number;
}

export interface ProfileSummary {
  id: string;
  Owner: OwnerInfo;
  LastUpdated: string;
  Name: string;
  AircraftType: AircraftType;
  Engines: number;
  IsPublished: boolean;
  Notes: string | null;
}

export interface Profile {
  id: string | null;
  Owner: OwnerInfo;
  LastUpdated: string;
  Name: string;
  AircraftType: AircraftType;
  Engines: number;
  IsPublished: boolean;
  Notes: string | null;
  ForkedFrom: string | null;

  // Piston only
  Cylinders: number;
  FADEC: boolean;
  Turbocharged: boolean;
  ConstantSpeed: boolean;
  VacuumPSIRange: VacuumPSIRange;
  ManifoldPressure: Gauge;
  CHT: Gauge;
  EGT: Gauge;
  TIT: Gauge;
  Load: Gauge;

  // Turbo only
  Torque: Gauge;
  NG: Gauge;

  // Turbo + Jet
  ITT: Gauge;

  // Common to all
  TemperaturesInFahrenheit: boolean;
  RPM: Gauge;
  Fuel: Gauge;
  FuelFlow: Gauge;
  OilPressure: Gauge;
  OilTemperature: Gauge;

  DisplayElevatorTrim: boolean;
  ElevatorTrimTakeOffRange: SettingRange;
  DisplayRudderTrim: boolean;
  RudderTrimTakeOffRange: SettingRange;
  DisplayFlapsIndicator: boolean;
  FlapsRange: FlapsRange;
  VSpeeds: VSpeeds;
}
