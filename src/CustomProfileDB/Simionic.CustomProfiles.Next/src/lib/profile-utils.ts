import {
  Profile,
  ProfileSummary,
  AircraftType,
  PublishedStatus,
  RangeColour,
  Gauge,
  GaugeRange,
} from "@/types";

export function fixUpGauges(profile: Profile): void {
  profile.ManifoldPressure.AllowDecimals = true;
  profile.FuelFlow.AllowDecimals = true;
  profile.OilPressure.AllowDecimals = true;

  fixRanges(profile.ManifoldPressure);
  fixRanges(profile.FuelFlow);
  fixRanges(profile.OilPressure);
}

function fixRanges(gauge: Gauge): void {
  for (let i = 0; i < 4; i++) {
    gauge.Ranges[i].AllowDecimals = true;
  }
}

export function filterByPublished(profiles: ProfileSummary[]): ProfileSummary[] {
  return profiles.filter((x) => x.IsPublished);
}

export function filterByType(profiles: ProfileSummary[], type: AircraftType): ProfileSummary[] {
  return profiles.filter((x) => x.AircraftType === type);
}

export function filterByEngineCount(profiles: ProfileSummary[], engines: number): ProfileSummary[] {
  return profiles.filter((x) => x.Engines === engines);
}

export function filterByOwner(profiles: ProfileSummary[], ownerId: string): ProfileSummary[] {
  return profiles.filter((x) => x.Owner?.Id != null && x.Owner.Id === ownerId);
}

export function filterBySearch(profiles: ProfileSummary[], searchTerms: string): ProfileSummary[] {
  if (!searchTerms?.trim()) return profiles;

  const terms = searchTerms.split(/[\s,]+/);
  let output = profiles;
  for (const term of terms) {
    const lower = term.toLowerCase();
    output = output.filter(
      (x) =>
        (x.Name && x.Name.toLowerCase().includes(lower)) ||
        (x.Owner?.Name && x.Owner.Name.toLowerCase().includes(lower)) ||
        AircraftType[x.AircraftType]?.toLowerCase().includes(lower)
    );
  }
  return output;
}

export function filterProfiles(
  profiles: ProfileSummary[],
  published: PublishedStatus,
  type: AircraftType | null,
  engines: number | null,
  ownerId: string | null,
  ownerOnly: boolean,
  searchTerms: string | null
): ProfileSummary[] {
  let output: ProfileSummary[];

  switch (published) {
    case PublishedStatus.Unpublished:
      output = profiles.filter((x) => !x.IsPublished);
      break;
    case PublishedStatus.PublishedOwner:
      output = profiles.filter((x) => x.IsPublished || x.Owner?.Id === ownerId);
      break;
    case PublishedStatus.UnpublishedOwner:
      output = profiles.filter((x) => !x.IsPublished && x.Owner?.Id === ownerId);
      break;
    default:
      output = profiles.filter((x) => x.IsPublished);
      break;
  }

  if (type != null) output = filterByType(output, type);
  if (engines != null) output = filterByEngineCount(output, engines);
  if (ownerId != null && ownerOnly) output = filterByOwner(output, ownerId);
  if (searchTerms != null) output = filterBySearch(output, searchTerms);

  return output.sort(
    (a, b) => new Date(b.LastUpdated).getTime() - new Date(a.LastUpdated).getTime()
  );
}

export function createDefaultGauge(
  name: string,
  min: number | null = null,
  max: number | null = null,
  options: {
    fuelInGallons?: boolean | null;
    capacityForSingleTank?: number | null;
    torqueInFootPounds?: boolean | null;
    maxPower?: number | null;
    allowDecimals?: boolean;
  } = {}
): Gauge {
  const ranges: GaugeRange[] = Array.from({ length: 4 }, () => ({
    Colour: RangeColour.None,
    Min: 0,
    Max: 0,
    AllowDecimals: options.allowDecimals ?? false,
  }));

  return {
    Name: name,
    Min: min,
    Max: max,
    FuelInGallons: options.fuelInGallons ?? null,
    CapacityForSingleTank: options.capacityForSingleTank ?? null,
    TorqueInFootPounds: options.torqueInFootPounds ?? null,
    MaxPower: options.maxPower ?? null,
    Ranges: ranges,
    AllowDecimals: options.allowDecimals ?? false,
  };
}

export function createDefaultProfile(): Profile {
  return {
    id: null,
    Owner: { Id: null, Name: null },
    LastUpdated: new Date().toISOString(),
    Name: "New Profile",
    AircraftType: AircraftType.Piston,
    Engines: 1,
    IsPublished: false,
    Notes: null,
    ForkedFrom: null,
    Cylinders: 4,
    FADEC: false,
    Turbocharged: false,
    ConstantSpeed: false,
    VacuumPSIRange: { Min: 0, Max: 0, GreenStart: 0, GreenEnd: 0 },
    ManifoldPressure: createDefaultGauge("Manifold Pressure (inHg)", 0, 0, { allowDecimals: true }),
    CHT: createDefaultGauge("CHT (\u00b0F)", 0, 0),
    EGT: createDefaultGauge("EGT (\u00b0F)", 0, 0),
    TIT: createDefaultGauge("TIT (\u00b0F)", 0, 0),
    Load: createDefaultGauge("Load %"),
    Torque: createDefaultGauge("Torque (FT-LB)", 0, 0, { torqueInFootPounds: true }),
    NG: createDefaultGauge("NG (RPM%)", null, null),
    ITT: createDefaultGauge("ITT (\u00b0F)", 0, 0),
    TemperaturesInFahrenheit: true,
    RPM: createDefaultGauge("RPM", null, 0),
    Fuel: createDefaultGauge("Fuel", undefined, undefined, { fuelInGallons: true, capacityForSingleTank: 0 }),
    FuelFlow: createDefaultGauge("Fuel Flow (GPH)", null, 0, { allowDecimals: true }),
    OilPressure: createDefaultGauge("Oil Pressure (PSI)", null, 0, { allowDecimals: true }),
    OilTemperature: createDefaultGauge("Oil Temp (\u00b0F)", 0, 0),
    DisplayElevatorTrim: false,
    ElevatorTrimTakeOffRange: { Min: 0, Max: 0 },
    DisplayRudderTrim: false,
    RudderTrimTakeOffRange: { Min: 0, Max: 0 },
    DisplayFlapsIndicator: false,
    FlapsRange: {
      Markings: ["UP", null, null, null, null, "F"],
      Positions: [0, null, null, null, null, 100],
    },
    VSpeeds: { Vs0: 0, Vs1: 0, Vfe: 0, Vno: 0, Vne: 0, Vglide: 0, Vr: 0, Vx: 0, Vy: 0 },
  };
}

export function getAircraftTypeImage(type: AircraftType): string {
  switch (type) {
    case AircraftType.Piston:
      return "/img/piston.jpg";
    case AircraftType.Turboprop:
      return "/img/turboprop.jpg";
    case AircraftType.Jet:
      return "/img/jet.jpg";
    default:
      return "";
  }
}

export function getAircraftTypeName(type: AircraftType): string {
  return AircraftType[type] ?? "Unknown";
}
