export interface BodyMetric {
  id: number;
  date: string;
  weightKg: number | null;
  bodyFatPercentage: number | null;
  muscleMassKg: number | null;
  chestCm: number | null;
  waistCm: number | null;
  hipsCm: number | null;
  bicepCm: number | null;
  thighCm: number | null;
  notes: string;
}