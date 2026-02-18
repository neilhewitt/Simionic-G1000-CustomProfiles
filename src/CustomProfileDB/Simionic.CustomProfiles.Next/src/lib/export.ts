import { Profile } from "@/types";

export function exportProfileAsJson(profile: Profile): void {
  const exportData = { ...profile };

  // Strip owner details and ID for export
  exportData.Owner = { Id: null, Name: null };
  exportData.id = null;

  const json = JSON.stringify(exportData, null, 2);
  const blob = new Blob([json], { type: "application/json" });
  const url = URL.createObjectURL(blob);

  const a = document.createElement("a");
  a.href = url;
  a.download = `${profile.Name}.json`;
  a.click();

  URL.revokeObjectURL(url);
  a.remove();
}
