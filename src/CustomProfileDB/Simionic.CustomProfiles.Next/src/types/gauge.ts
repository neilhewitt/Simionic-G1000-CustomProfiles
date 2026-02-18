import { RangeColour } from "./enums";

export interface GaugeRange {
  Colour: RangeColour;
  Min: number;
  Max: number;
  AllowDecimals?: boolean;
}

export interface Gauge {
  Name: string;
  Min: number | null;
  Max: number | null;
  FuelInGallons?: boolean | null;
  CapacityForSingleTank?: number | null;
  TorqueInFootPounds?: boolean | null;
  MaxPower?: number | null;
  Ranges: GaugeRange[];
  AllowDecimals: boolean;
}
