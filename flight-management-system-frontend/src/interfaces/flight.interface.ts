export interface IFlight {
    flightId: number;
    flightNumber: string;
    departureDate: Date;
    departureLocation: string;
    arrivalLocation: string;
    planeType: PlaneType;

}

export enum PlaneType{
    EMBRAER = "Embraer",
    AIRBUS = "Airbus",
    BOEING = "Boeing"
}