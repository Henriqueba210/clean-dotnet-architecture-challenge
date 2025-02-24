import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from '../../../environments/environment';

export interface LocationCodes {
    adminDistrict: string | null;
    adminCounty: string | null;
    adminWard: string | null;
    parish: string | null;
    parliamentaryConstituency: string | null;
}

export interface DistanceToHeathrow {
    kilometers: number;
    miles: number;
}

export interface Location {
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
    codes: LocationCodes | null;
    distanceToHeathrowAirport: DistanceToHeathrow;
}

@Injectable({
    providedIn: 'root'
})
export class LocationService {
    private readonly apiUrl = `${environment.apiUrl}/api/location/address`;
    
    constructor(private http: HttpClient) {}

    getLocation(postcode: string): Observable<Location> {
        return this.http.get<Location>(`${this.apiUrl}/${postcode}`);
    }
}