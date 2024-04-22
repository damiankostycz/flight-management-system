import { PlaneType } from "../flight.interface";

export interface IAddFlightParams {
    flightNumber: string;
    departureDate: Date;
    departureLocation: string;
    arrivalLocation: string;
    planeType: PlaneType;
}