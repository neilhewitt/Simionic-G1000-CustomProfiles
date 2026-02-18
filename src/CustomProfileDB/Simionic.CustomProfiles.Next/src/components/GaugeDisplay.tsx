"use client";

import { useState } from "react";
import { Gauge, RangeColour } from "@/types";

interface GaugeDisplayProps {
  gauge: Gauge;
  gaugeType?: "Standard" | "Fuel" | "Torque" | "Load" | "NG";
  hasRange?: boolean;
  editing?: boolean;
  onChange?: (gauge: Gauge) => void;
}

function getColourName(colour: RangeColour): string {
  switch (colour) {
    case RangeColour.Green: return "green";
    case RangeColour.Yellow: return "yellow";
    case RangeColour.Red: return "red";
    default: return "none";
  }
}

function ColourIndicator({
  range,
  index,
  editing,
  onChange,
}: {
  range: { Colour: RangeColour; Min: number; Max: number };
  index: number;
  editing: boolean;
  onChange: (index: number, colour: RangeColour) => void;
}) {
  const [showDropdown, setShowDropdown] = useState(false);
  const colourName = getColourName(range.Colour);

  return (
    <div className="col">
      {/* eslint-disable-next-line @next/next/no-img-element */}
      <div
        className="profile-indicator"
        role={editing ? "button" : undefined}
        onClick={() => editing && setShowDropdown(!showDropdown)}
      >
        <img
          src={`/img/${colourName}.jpg`}
          alt={colourName}
          style={{ height: "125%", width: "100%", objectFit: "cover" }}
        />
      </div>
      {editing && showDropdown && (
        <ul className="dropdown-menu show px-0 py-0 w100">
          <li><div className="profile-indicator none" onClick={() => { onChange(index, RangeColour.None); setShowDropdown(false); }} /></li>
          <li><div className="profile-indicator green" onClick={() => { onChange(index, RangeColour.Green); setShowDropdown(false); }} /></li>
          <li><div className="profile-indicator yellow" onClick={() => { onChange(index, RangeColour.Yellow); setShowDropdown(false); }} /></li>
          <li><div className="profile-indicator red" onClick={() => { onChange(index, RangeColour.Red); setShowDropdown(false); }} /></li>
        </ul>
      )}
    </div>
  );
}

export default function GaugeDisplay({
  gauge,
  gaugeType = "Standard",
  hasRange = false,
  editing = false,
  onChange,
}: GaugeDisplayProps) {
  if (!gauge) return null;

  function updateGauge(updates: Partial<Gauge>) {
    if (onChange) {
      onChange({ ...gauge, ...updates });
    }
  }

  function updateRangeColour(index: number, colour: RangeColour) {
    const newRanges = [...gauge.Ranges];
    newRanges[index] = { ...newRanges[index], Colour: colour };
    updateGauge({ Ranges: newRanges });
  }

  function updateRangeValue(index: number, field: "Min" | "Max", value: string) {
    const newRanges = [...gauge.Ranges];
    newRanges[index] = { ...newRanges[index], [field]: Number(value) || 0 };
    updateGauge({ Ranges: newRanges });
  }

  function isSelectedButton(active: boolean): string {
    return active ? "btn-primary" : "btn-secondary";
  }

  return (
    <div>
      {/* Header row */}
      <div className="row justify-content-center">
        <div className={`${gaugeType === "NG" ? "col-12" : "col-3"} pt-2 pr-5`}>
          <p className="text-black font-weight-bold">{gauge.Name}</p>
        </div>

        {gaugeType === "Fuel" && (
          <>
            <div className="col-2">
              <div className="form-group">
                <div className="btn-group">
                  <button
                    className={`btn btn-large ${isSelectedButton(!!gauge.FuelInGallons)} mt-1 mb-1 shadow-none`}
                    onClick={() => editing && updateGauge({ FuelInGallons: true })}
                    disabled={!editing}
                  >
                    Gal
                  </button>
                  <button
                    className={`btn btn-large ${isSelectedButton(!gauge.FuelInGallons)} mt-1 mb-1 shadow-none`}
                    onClick={() => editing && updateGauge({ FuelInGallons: false })}
                    disabled={!editing}
                  >
                    Lb
                  </button>
                </div>
              </div>
            </div>
            <div className="col-7">
              <div className="form-group">
                <label className="form-label text-black font-weight-bold mt-1">
                  Capacity for a single tank{" "}
                  <input
                    type="text"
                    className="input-text ml-2 custom-profile-textbox"
                    value={gauge.CapacityForSingleTank ?? ""}
                    onChange={(e) => updateGauge({ CapacityForSingleTank: Number(e.target.value) || 0 })}
                    disabled={!editing}
                  />
                </label>
              </div>
            </div>
          </>
        )}

        {gaugeType === "Standard" && hasRange && (
          <div className="col-9 pt-1">
            <div className="form-group">
              <label className="form-label text-black font-weight-bold">
                Range
                <input
                  type="text"
                  className="input-text ml-2 custom-profile-textbox"
                  value={gauge.Min ?? ""}
                  onChange={(e) => updateGauge({ Min: Number(e.target.value) || 0 })}
                  disabled={!editing}
                />
                <span>~</span>
                <input
                  type="text"
                  className="input-text custom-profile-textbox"
                  value={gauge.Max ?? ""}
                  onChange={(e) => updateGauge({ Max: Number(e.target.value) || 0 })}
                  disabled={!editing}
                />
              </label>
            </div>
          </div>
        )}

        {gaugeType === "Standard" && !hasRange && (
          <div className="col-9 pt-1">
            <div className="form-group">
              <label className="form-label text-black font-weight-bold">
                Max
                <input
                  type="text"
                  className="input-text ml-2 custom-profile-textbox"
                  value={gauge.Max ?? ""}
                  onChange={(e) => updateGauge({ Max: Number(e.target.value) || 0 })}
                  disabled={!editing}
                />
              </label>
            </div>
          </div>
        )}

        {gaugeType === "Load" && (
          <div className="col-9">
            <div className="form-group">
              <label className="form-label text-black font-weight-bold">
                Max power (watts)
                <input
                  type="text"
                  className="input-text ml-2 custom-profile-textbox-wide"
                  value={gauge.MaxPower ?? ""}
                  onChange={(e) => updateGauge({ MaxPower: Number(e.target.value) || 0 })}
                  disabled={!editing}
                />
              </label>
            </div>
          </div>
        )}

        {gaugeType === "Torque" && (
          <>
            <div className="col-6 pt-1">
              <div className="form-group">
                <label className="form-label text-black font-weight-bold">
                  Range
                  <input
                    type="text"
                    className="input-text ml-2 custom-profile-textbox"
                    value={gauge.Min ?? ""}
                    onChange={(e) => updateGauge({ Min: Number(e.target.value) || 0 })}
                    disabled={!editing}
                  />
                  <span>~</span>
                  <input
                    type="text"
                    className="input-text custom-profile-textbox"
                    value={gauge.Max ?? ""}
                    onChange={(e) => updateGauge({ Max: Number(e.target.value) || 0 })}
                    disabled={!editing}
                  />
                </label>
              </div>
            </div>
            <div className="col-3">
              <div className="form-group">
                <div className="btn-group">
                  <button
                    className={`btn btn-large ${isSelectedButton(!!gauge.TorqueInFootPounds)} mt-1 mb-1 shadow-none`}
                    onClick={() => editing && updateGauge({ TorqueInFootPounds: true })}
                    disabled={!editing}
                  >
                    Value
                  </button>
                  <button
                    className={`btn btn-large ${isSelectedButton(!gauge.TorqueInFootPounds)} mt-1 mb-1 shadow-none`}
                    onClick={() => editing && updateGauge({ TorqueInFootPounds: false })}
                    disabled={!editing}
                  >
                    Percentage
                  </button>
                </div>
              </div>
            </div>
          </>
        )}
      </div>

      {/* Colour indicator bars */}
      <div className="row justify-content-center mb-0">
        {gauge.Ranges.map((range, i) => (
          <ColourIndicator
            key={i}
            range={range}
            index={i}
            editing={editing}
            onChange={updateRangeColour}
          />
        ))}
      </div>

      {/* Range value inputs */}
      <div className="row justify-content-center mb-5">
        {gauge.Ranges.map((range, i) => (
          <div key={i} className="col">
            <input
              type="text"
              className="input-text custom-profile-valuebox"
              value={range.Min}
              onChange={(e) => updateRangeValue(i, "Min", e.target.value)}
              disabled={!editing}
            />
            <input
              type="text"
              className="input-text custom-profile-valuebox"
              value={range.Max}
              onChange={(e) => updateRangeValue(i, "Max", e.target.value)}
              disabled={!editing}
            />
          </div>
        ))}
      </div>
    </div>
  );
}
