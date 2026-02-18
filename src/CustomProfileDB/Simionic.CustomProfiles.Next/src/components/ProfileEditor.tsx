"use client";

import { Fragment } from "react";
import { Profile, AircraftType, Gauge } from "@/types";
import GaugeDisplay from "./GaugeDisplay";

interface ProfileEditorProps {
  profile: Profile;
  editing: boolean;
  onChange?: (profile: Profile) => void;
}

export default function ProfileEditor({ profile, editing, onChange }: ProfileEditorProps) {
  function update(updates: Partial<Profile>) {
    if (onChange) {
      onChange({ ...profile, ...updates });
    }
  }

  function updateGauge(key: keyof Profile, gauge: Gauge) {
    update({ [key]: gauge });
  }

  function setTempScale(fahrenheit: boolean) {
    const newTemp = "\u00b0" + (fahrenheit ? "F" : "C");
    const oldTemp = "\u00b0" + (fahrenheit ? "C" : "F");

    const gaugeKeys: (keyof Profile)[] = [
      "CHT", "EGT", "Torque", "NG", "ITT", "ManifoldPressure",
      "Load", "RPM", "Fuel", "TIT", "FuelFlow", "OilPressure", "OilTemperature",
    ];

    const updates: Partial<Profile> = { TemperaturesInFahrenheit: fahrenheit };
    for (const key of gaugeKeys) {
      const gauge = profile[key] as Gauge;
      if (gauge?.Name) {
        (updates as any)[key] = { ...gauge, Name: gauge.Name.replace(oldTemp, newTemp) };
      }
    }
    update(updates);
  }

  function setFADEC(fadec: boolean) {
    update({ FADEC: fadec, ConstantSpeed: fadec ? false : profile.ConstantSpeed });
  }

  function isSelectedButton(active: boolean): string {
    return active ? "btn-primary" : "btn-secondary";
  }

  return (
    <div>
      {/* Aircraft type */}
      <div className="row">
        <div className="col-3">
          <label className="form-label text-black font-weight-bold pt-2">Type</label>
        </div>
        <div className="col-9">
          <div className="form-group">
            <div className="btn-group">
              {[AircraftType.Piston, AircraftType.Turboprop, AircraftType.Jet].map((type) => (
                <button
                  key={type}
                  className={`btn btn-large ${isSelectedButton(profile.AircraftType === type)} mt-1 mb-1 shadow-none`}
                  onClick={() => update({ AircraftType: type })}
                  disabled={!editing}
                >
                  {AircraftType[type]}
                </button>
              ))}
            </div>
            <div className="btn-group">
              <button
                className={`btn btn-large ${isSelectedButton(profile.Engines === 1)} mt-1 mb-1 shadow-none`}
                onClick={() => update({ Engines: 1 })}
                disabled={!editing}
              >
                Single
              </button>
              <button
                className={`btn btn-large ${isSelectedButton(profile.Engines === 2)} mt-1 mb-1 shadow-none`}
                onClick={() => update({ Engines: 2 })}
                disabled={!editing}
              >
                Twin
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Piston-specific options */}
      {profile.AircraftType === AircraftType.Piston && (
        <>
          <div className="row">
            <div className="col-3">
              <label className="form-label text-black font-weight-bold pt-2">Cylinders</label>
            </div>
            <div className="col-9">
              <div className="form-group">
                <div className="btn-group">
                  <button
                    className={`btn btn-large ${isSelectedButton(profile.Cylinders === 4)} mt-1 mb-1 shadow-none`}
                    onClick={() => update({ Cylinders: 4 })}
                    disabled={!editing}
                  >
                    4
                  </button>
                  <button
                    className={`btn btn-large ${isSelectedButton(profile.Cylinders === 6)} mt-1 mb-1 shadow-none`}
                    onClick={() => update({ Cylinders: 6 })}
                    disabled={!editing}
                  >
                    6
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-3 pt-1">
              <label className="form-label text-black font-weight-bold">FADEC</label>
            </div>
            <div className="col-9">
              <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.FADEC} onChange={(e) => setFADEC(e.target.checked)} disabled={!editing} />
            </div>
          </div>
          <div className="row">
            <div className="col-3 pt-1">
              <label className="form-label text-black font-weight-bold">Turbocharged</label>
            </div>
            <div className="col-9">
              <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.Turbocharged} onChange={(e) => update({ Turbocharged: e.target.checked })} disabled={!editing} />
            </div>
          </div>
          <div className="row">
            <div className="col-3 pt-1">
              <label className="form-label text-black font-weight-bold">Constant-speed</label>
            </div>
            <div className="col-9">
              <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.ConstantSpeed} onChange={(e) => update({ ConstantSpeed: e.target.checked })} disabled={!editing || profile.FADEC} />
            </div>
          </div>
        </>
      )}

      {/* Temperature scale */}
      <div className="row pt-3">
        <div className="col-3 pt-3 pt-1">
          <label className="form-label text-black font-weight-bold mr-2">Temperature</label>
        </div>
        <div className="col-9 pt-2">
          <div className="form-group">
            <div className="btn-group">
              <button
                className={`btn btn-large ${isSelectedButton(profile.TemperaturesInFahrenheit)} mt-1 mb-1 shadow-none`}
                onClick={() => setTempScale(true)}
                disabled={!editing}
              >
                &deg;F
              </button>
              <button
                className={`btn btn-large ${isSelectedButton(!profile.TemperaturesInFahrenheit)} mt-1 mb-1 shadow-none`}
                onClick={() => setTempScale(false)}
                disabled={!editing}
              >
                &deg;C
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Gauges - conditional on aircraft type */}
      {profile.AircraftType !== AircraftType.Piston && (
        <GaugeDisplay gauge={profile.ITT} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("ITT", g)} />
      )}
      {profile.AircraftType === AircraftType.Turboprop && (
        <GaugeDisplay gauge={profile.Torque} gaugeType="Torque" hasRange editing={editing} onChange={(g) => updateGauge("Torque", g)} />
      )}
      {profile.AircraftType === AircraftType.Piston && profile.ConstantSpeed && (
        <GaugeDisplay gauge={profile.ManifoldPressure} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("ManifoldPressure", g)} />
      )}
      {profile.AircraftType === AircraftType.Piston && profile.FADEC && (
        <GaugeDisplay gauge={profile.Load} gaugeType="Load" editing={editing} onChange={(g) => updateGauge("Load", g)} />
      )}
      {profile.AircraftType !== AircraftType.Jet && (
        <GaugeDisplay gauge={profile.RPM} gaugeType="Standard" editing={editing} onChange={(g) => updateGauge("RPM", g)} />
      )}
      {profile.AircraftType === AircraftType.Piston && profile.Turbocharged && (
        <GaugeDisplay gauge={profile.TIT} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("TIT", g)} />
      )}
      {profile.AircraftType === AircraftType.Turboprop && (
        <GaugeDisplay gauge={profile.NG} gaugeType="NG" editing={editing} onChange={(g) => updateGauge("NG", g)} />
      )}

      <GaugeDisplay gauge={profile.Fuel} gaugeType="Fuel" editing={editing} onChange={(g) => updateGauge("Fuel", g)} />
      <GaugeDisplay gauge={profile.FuelFlow} gaugeType="Standard" editing={editing} onChange={(g) => updateGauge("FuelFlow", g)} />

      {/* Vacuum PSI - Piston only */}
      {profile.AircraftType === AircraftType.Piston && (
        <div className="row mb-5">
          <div className="col-2 pt-2 pr-5"><p className="text-black font-weight-bold">Vacuum (PSI)</p></div>
          <div className="col-md-auto pt-2 pr-1"><p className="text-black font-weight-bold">Range</p></div>
          <div className="col-md-auto pt-1">
            <div className="form-group">
              <label className="form-label text-black font-weight-bold">
                <input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.VacuumPSIRange.Min} onChange={(e) => update({ VacuumPSIRange: { ...profile.VacuumPSIRange, Min: Number(e.target.value) || 0 } })} disabled={!editing} />
                <span>~</span>
                <input type="text" className="input-text custom-profile-textbox" value={profile.VacuumPSIRange.Max} onChange={(e) => update({ VacuumPSIRange: { ...profile.VacuumPSIRange, Max: Number(e.target.value) || 0 } })} disabled={!editing} />
              </label>
            </div>
          </div>
          <div className="col-md-auto pt-2 pl-5 pr-1"><p className="text-black font-weight-bold">Green</p></div>
          <div className="col-md-auto pt-1">
            <div className="form-group">
              <label className="form-label text-black font-weight-bold">
                <input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.VacuumPSIRange.GreenStart} onChange={(e) => update({ VacuumPSIRange: { ...profile.VacuumPSIRange, GreenStart: Number(e.target.value) || 0 } })} disabled={!editing} />
                <span>~</span>
                <input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.VacuumPSIRange.GreenEnd} onChange={(e) => update({ VacuumPSIRange: { ...profile.VacuumPSIRange, GreenEnd: Number(e.target.value) || 0 } })} disabled={!editing} />
              </label>
            </div>
          </div>
        </div>
      )}

      <GaugeDisplay gauge={profile.OilPressure} gaugeType="Standard" editing={editing} onChange={(g) => updateGauge("OilPressure", g)} />
      <GaugeDisplay gauge={profile.OilTemperature} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("OilTemperature", g)} />

      {profile.AircraftType === AircraftType.Piston && (
        <>
          <GaugeDisplay gauge={profile.CHT} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("CHT", g)} />
          <GaugeDisplay gauge={profile.EGT} gaugeType="Standard" hasRange editing={editing} onChange={(g) => updateGauge("EGT", g)} />
        </>
      )}

      {/* Elevator Trim */}
      <div className="row mb-3">
        <div className="col-2 pt-1">
          <label className="form-label text-black font-weight-bold">Elevator Trim</label>
        </div>
        <div className="col-2">
          <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.DisplayElevatorTrim} onChange={(e) => update({ DisplayElevatorTrim: e.target.checked })} disabled={!editing} />
        </div>
        <div className="col-md-auto pt-1 ml-5">
          <div className="form-group mb-0">
            <label className={`form-label ${profile.DisplayElevatorTrim ? "text-black" : "text-muted"} font-weight-bold`}>
              T/O Range (0-100)
              <input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.ElevatorTrimTakeOffRange.Min} onChange={(e) => update({ ElevatorTrimTakeOffRange: { ...profile.ElevatorTrimTakeOffRange, Min: Number(e.target.value) || 0 } })} disabled={!editing || !profile.DisplayElevatorTrim} />
              ~<input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.ElevatorTrimTakeOffRange.Max} onChange={(e) => update({ ElevatorTrimTakeOffRange: { ...profile.ElevatorTrimTakeOffRange, Max: Number(e.target.value) || 0 } })} disabled={!editing || !profile.DisplayElevatorTrim} />
            </label>
          </div>
        </div>
      </div>

      {/* Rudder Trim */}
      <div className="row mb-3">
        <div className="col-2 pt-1">
          <label className="form-label text-black font-weight-bold">Rudder Trim</label>
        </div>
        <div className="col-2">
          <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.DisplayRudderTrim} onChange={(e) => update({ DisplayRudderTrim: e.target.checked })} disabled={!editing} />
        </div>
        <div className="col-md-auto pt-1 ml-5">
          <div className="form-group mb-0">
            <label className={`form-label ${profile.DisplayRudderTrim ? "text-black" : "text-muted"} font-weight-bold`}>
              T/O Range (0-100)
              <input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.RudderTrimTakeOffRange.Min} onChange={(e) => update({ RudderTrimTakeOffRange: { ...profile.RudderTrimTakeOffRange, Min: Number(e.target.value) || 0 } })} disabled={!editing || !profile.DisplayRudderTrim} />
              ~<input type="text" className="input-text ml-2 custom-profile-textbox" value={profile.RudderTrimTakeOffRange.Max} onChange={(e) => update({ RudderTrimTakeOffRange: { ...profile.RudderTrimTakeOffRange, Max: Number(e.target.value) || 0 } })} disabled={!editing || !profile.DisplayRudderTrim} />
            </label>
          </div>
        </div>
      </div>

      {/* Flap Indicator */}
      <div className="row mb-4">
        <div className="col-2 pt-1">
          <label className="form-label text-black font-weight-bold">Flap Indicator</label>
        </div>
        <div className="col-2">
          <input className="form-check-input custom-profile-checkbox shadow-none" type="checkbox" checked={profile.DisplayFlapsIndicator} onChange={(e) => update({ DisplayFlapsIndicator: e.target.checked })} disabled={!editing} />
        </div>
      </div>

      {/* Flap Markings */}
      <div className="row">
        <div className="col-2" />
        <div className="col-2 pt-1">
          <label className={`form-label ${profile.DisplayFlapsIndicator ? "text-black" : "text-muted"} font-weight-bold`}>Markings</label>
        </div>
        <div className="col-8 pt-1">
          <div className="form-group mb-0">
            {profile.FlapsRange.Markings.map((m, i) => (
              <input
                key={i}
                type="text"
                className={`input-text custom-profile-textbox${i > 0 ? " ml-2" : ""}`}
                value={m ?? ""}
                onChange={(e) => {
                  const newMarkings = [...profile.FlapsRange.Markings];
                  newMarkings[i] = e.target.value || null;
                  update({ FlapsRange: { ...profile.FlapsRange, Markings: newMarkings } });
                }}
                disabled={!editing || !profile.DisplayFlapsIndicator}
              />
            ))}
          </div>
        </div>
      </div>

      {/* Flap Positions */}
      <div className="row mb-4">
        <div className="col-2" />
        <div className="col-2 pt-1">
          <label className={`form-label ${profile.DisplayFlapsIndicator ? "text-black" : "text-muted"} font-weight-bold`}>Positions</label>
        </div>
        <div className="col-8 pt-1">
          <div className="form-group mb-0">
            {profile.FlapsRange.Positions.map((p, i) => (
              <input
                key={i}
                type="text"
                className={`input-text custom-profile-textbox${i > 0 ? " ml-2" : ""}`}
                placeholder="%"
                value={p ?? ""}
                onChange={(e) => {
                  const newPositions = [...profile.FlapsRange.Positions];
                  newPositions[i] = e.target.value ? Number(e.target.value) : null;
                  update({ FlapsRange: { ...profile.FlapsRange, Positions: newPositions } });
                }}
                disabled={!editing || !profile.DisplayFlapsIndicator || (i === 5)}
              />
            ))}
          </div>
        </div>
      </div>

      {/* V-Speeds */}
      <div className="row mb-1">
        {(["Vs0", "Vs1", "Vfe", "Vno", "Vne"] as const).map((key) => (
          <Fragment key={key}>
            <div className="col-1 text-end pr-1">
              <label className="form-label text-black font-weight-bold mb-0 mt-1">{key}</label>
            </div>
            <div className="col-1 px-0">
              <input
                type="text"
                className="input-text ml-1 custom-profile-textbox"
                value={profile.VSpeeds[key]}
                onChange={(e) => update({ VSpeeds: { ...profile.VSpeeds, [key]: Number(e.target.value) || 0 } })}
                disabled={!editing}
              />
            </div>
          </Fragment>
        ))}
      </div>
      <div className="row mb-4">
        <div className="col-2" />
        {(["Vglide", "Vr", "Vx", "Vy"] as const).map((key) => (
          <Fragment key={key}>
            <div className="col-1 text-end pr-1">
              <label className="form-label text-black font-weight-bold mb-0 mt-1">{key === "Vglide" ? "Vg" : key}</label>
            </div>
            <div className="col-1 px-0">
              <input
                type="text"
                className="input-text ml-1 custom-profile-textbox"
                value={profile.VSpeeds[key]}
                onChange={(e) => update({ VSpeeds: { ...profile.VSpeeds, [key]: Number(e.target.value) || 0 } })}
                disabled={!editing}
              />
            </div>
          </Fragment>
        ))}
      </div>

      {/* Notes (edit mode only) */}
      {editing && (
        <div className="row mt-5 mb-3">
          <div className="col-12">
            <label className="form-label text-black font-weight-bold">Notes</label><br />
            <textarea
              className="form-control input-text shadow-none w-auto p-3"
              style={{ resize: "none", minWidth: "100%" }}
              rows={5}
              maxLength={2000}
              placeholder="Any notes about this profile you want users to see"
              value={profile.Notes ?? ""}
              onChange={(e) => update({ Notes: e.target.value })}
            />
          </div>
        </div>
      )}
    </div>
  );
}
