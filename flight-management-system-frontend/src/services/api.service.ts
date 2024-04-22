import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { IFlight } from "src/interfaces/flight.interface";
import { IAddFlightParams } from "src/interfaces/api/add-flight.interface";

@Injectable({
    providedIn: 'root'   
})
export class ApiService{
    private baseUrl = "http://localhost:5019/Flight"
    //token is left for development purposes, ideally it would be obtained from an authentication server or another provider
    private jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRhbWlhIiwic3ViIjoiZGFtaWEiLCJqdGkiOiJlZmY2NzkyMSIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjEwNjQyIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6MCIsImh0dHA6Ly9sb2NhbGhvc3Q6NTAxOSJdLCJuYmYiOjE3MTM1NTU4NDEsImV4cCI6MTcyMTQxODI0MSwiaWF0IjoxNzEzNTU1ODQyLCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.R5jSEu_cXKOUGJ39wVXVgv5k2RwTiH8jUtg-7ptHkbw"

    constructor(private http: HttpClient) { }

    getAllFlights(): Promise<IFlight[]> {
      const url = `${this.baseUrl}/getAllFlights`;
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.jwtToken}`
      });
      return firstValueFrom(this.http.get<IFlight[]>(url, {headers}));
    }

    addFlight(params: IAddFlightParams): Promise<IFlight> {
      const url = `${this.baseUrl}/addFlight`;
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.jwtToken}`
      });
      return firstValueFrom(this.http.post<IFlight>(url, params,{headers}))
    }

    modifyFlight(flight: IFlight): Promise<IFlight>{
      const url = `${this.baseUrl}/modifyFlight`;
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.jwtToken}`
      });
      return firstValueFrom(this.http.put<IFlight>(url, flight, {headers}))
    }

    deleteFlight(flightId: number): Promise<string>{
      const url = `${this.baseUrl}/deleteFlight?id=${flightId}`;
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.jwtToken}`
      });
      return firstValueFrom(this.http.delete(url, { headers, responseType: 'text' }))}
}