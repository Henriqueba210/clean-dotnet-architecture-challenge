export interface DistanceToHeathrowAirport {
    kilometers: number;
    miles: number;
  }
  
  export interface Codes {
    adminDistrict: string | null;
    adminCounty: string | null;
    adminWard: string | null;
    parish: string | null;
    parliamentaryConstituency: string | null;
  }
  
  export interface AppLocation {
    postcode: string;
    quality: number;
    eastings: number | null;
    northings: number | null;
    country: string;
    nhsHa: string | null;
    longitude: number;
    latitude: number | null;
    region: string | null;
    adminDistrict: string | null;
    adminCounty: string | null;
    adminWard: string | null;
    codes: Codes | null;
    distanceToHeathrowAirport: DistanceToHeathrowAirport;
  }