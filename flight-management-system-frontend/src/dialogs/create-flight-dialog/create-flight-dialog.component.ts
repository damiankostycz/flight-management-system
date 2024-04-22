import { Component, Inject } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IAddFlightParams } from 'src/interfaces/api/add-flight.interface';
import { IFlight, PlaneType } from 'src/interfaces/flight.interface';

@Component({
  selector: 'app-create-flight-dialog',
  templateUrl: './create-flight-dialog.component.html',
  styleUrls: ['./create-flight-dialog.component.scss'],
  
})
export class CreateFlightDialogComponent {

  flightToCreate: IAddFlightParams;
  planeTypes: PlaneType[] = Object.values(PlaneType);

  constructor(
    public dialogRef: MatDialogRef<CreateFlightDialogComponent>,
    private _adapter: DateAdapter<MatDatepickerInputEvent<Date>>,

    @Inject(MAT_DIALOG_DATA) public data: {isEdit: boolean, flight?: IFlight},
  ) {
    this._adapter.setLocale('pl');

    if(data.flight && data.isEdit){
      this.flightToCreate={...data.flight}
    } else{
      this.flightToCreate={flightNumber: '', departureDate: new Date(), departureLocation:"", arrivalLocation: "", planeType: PlaneType.AIRBUS}
    }

  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  onClose(): void {
    if(this.data.isEdit){
      const editedFlight = {...this.flightToCreate, flightId: this.data.flight?.flightId}
      this.dialogRef.close(editedFlight);
    } else{
      this.dialogRef.close(this.flightToCreate);
    }
    
  }
  areFieldsValid(): boolean {
    return !!this.flightToCreate.flightNumber && 
           !!this.flightToCreate.departureDate && 
           !!this.flightToCreate.departureLocation && 
           !!this.flightToCreate.arrivalLocation && 
           !!this.flightToCreate.planeType;
  }
  

}
