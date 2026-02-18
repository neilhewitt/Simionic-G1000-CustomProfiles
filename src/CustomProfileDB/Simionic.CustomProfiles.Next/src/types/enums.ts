export enum AircraftType {
  Piston = 0,
  Turboprop = 1,
  Jet = 2,
}

export enum RangeColour {
  None = 0,
  Green = 1,
  Yellow = 2,
  Red = 3,
}

export enum GaugeType {
  Standard = "Standard",
  Fuel = "Fuel",
  Torque = "Torque",
  Load = "Load",
  NG = "NG",
}

export enum PublishedStatus {
  Published = "Published",
  Unpublished = "Unpublished",
  PublishedOwner = "PublishedOwner",
  UnpublishedOwner = "UnpublishedOwner",
  All = "All",
}
