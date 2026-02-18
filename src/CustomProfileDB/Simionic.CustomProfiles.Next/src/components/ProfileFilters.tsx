"use client";

import { AircraftType } from "@/types";

interface ProfileFiltersProps {
  searchTerm: string;
  onSearchChange: (term: string) => void;
  typeFilter: AircraftType | null;
  onTypeChange: (type: AircraftType | null) => void;
  engineFilter: number | null;
  onEngineChange: (engines: number | null) => void;
  isLoggedIn: boolean;
  onlyShowMine: boolean;
  onlyShowDrafts: boolean;
  onOwnerFilterChange: (mine: boolean, drafts: boolean) => void;
  onReset: () => void;
}

function getButtonClass(active: boolean): string {
  return `btn btn-large ${active ? "btn-primary" : "btn-secondary"} mt-1 mb-4 shadow-none`;
}

export default function ProfileFilters({
  searchTerm,
  onSearchChange,
  typeFilter,
  onTypeChange,
  engineFilter,
  onEngineChange,
  isLoggedIn,
  onlyShowMine,
  onlyShowDrafts,
  onOwnerFilterChange,
  onReset,
}: ProfileFiltersProps) {
  return (
    <div className="text-center">
      <div className="text-center">
        <label htmlFor="search-box" className="form-label text-black fw-bold">
          Filter:{" "}
          <input
            className="input-text"
            type="text"
            id="search-box"
            value={searchTerm}
            onChange={(e) => onSearchChange(e.target.value)}
          />
        </label>
      </div>
      <div className="btn-group">
        <button className={getButtonClass(typeFilter === null)} onClick={() => onTypeChange(null)}>All</button>
        <button className={getButtonClass(typeFilter === AircraftType.Piston)} onClick={() => onTypeChange(AircraftType.Piston)}>Piston</button>
        <button className={getButtonClass(typeFilter === AircraftType.Turboprop)} onClick={() => onTypeChange(AircraftType.Turboprop)}>Turbo<span className="d-none d-sm-inline">prop</span></button>
        <button className={getButtonClass(typeFilter === AircraftType.Jet)} onClick={() => onTypeChange(AircraftType.Jet)}>Jet</button>
      </div>
      <div className="btn-group">
        <button className={getButtonClass(engineFilter === null)} onClick={() => onEngineChange(null)}>Both</button>
        <button className={getButtonClass(engineFilter === 1)} onClick={() => onEngineChange(1)}>Single</button>
        <button className={getButtonClass(engineFilter === 2)} onClick={() => onEngineChange(2)}>Twin</button>
      </div>
      {isLoggedIn && (
        <div className="btn-group ml-1">
          <button className={getButtonClass(!onlyShowMine)} onClick={() => onOwnerFilterChange(false, false)}>All</button>
          <button className={getButtonClass(onlyShowMine && !onlyShowDrafts)} onClick={() => onOwnerFilterChange(true, false)}>Just mine</button>
          <button className={getButtonClass(onlyShowMine && onlyShowDrafts)} onClick={() => onOwnerFilterChange(true, true)}>Just drafts</button>
        </div>
      )}
      <button className="btn btn-large btn-primary mt-1 ml-1 mb-4 shadow-none" onClick={onReset}>Reset</button>
    </div>
  );
}
