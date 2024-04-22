import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateFlightDialogComponent } from 'src/dialogs/create-flight-dialog/create-flight-dialog.component';
import { IFlight } from 'src/interfaces/flight.interface';
import { ApiService } from 'src/services/api.service';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss']
})
export class FlightListComponent {
    flights: IFlight[];
    isLoading: boolean;
    constructor(private flightService: ApiService, private dialog: MatDialog){}

    ngOnInit(): void {
      this.loadFlights();
    }
  
    private loadFlights(): void {
      this.flightService.getAllFlights().then(
        flights => {
          this.flights = flights;
          this.isLoading = false;
        },
        error => {
          console.error(error);
          this.isLoading = false;
        }
      );
    }

    addFlight(): void {
      const dialogRef = this.dialog.open(CreateFlightDialogComponent, {
        data: {isEdit: false}
      });
      const sub = dialogRef.afterClosed().subscribe(params => {
        if(params){
        this.flightService.addFlight(params).then(
        (flight) => {
          this.flights.push(flight);
        },
        error => {
          console.error(error);
        })
        sub.unsubscribe();
      }
      });
  }

  editFlight(flight: IFlight): void {
    const dialogRef = this.dialog.open(CreateFlightDialogComponent, {
      data: {flight, isEdit: true}
    });
    const sub = dialogRef.afterClosed().subscribe(editedFlight => {
      debugger
      if(editedFlight){
        this.flightService.modifyFlight(editedFlight).then(
          (_flight) => {
            const index = this.flights.findIndex(f => f.flightId === _flight.flightId);
            if (index !== -1) {
              this.flights[index] = _flight;
            }
          },
        error => {
          console.error(error);
        })
        sub.unsubscribe();
      }
    })
  }

  deleteFlight(flightId: number): void {
    this.flightService.deleteFlight(flightId).then(
      () => {
        this.removeFlightFromList(flightId); 
      }
    )
  }

  private removeFlightFromList(id: number): void {
    const index = this.flights.findIndex(flight => flight.flightId === id);
    if (index !== -1) {
      this.flights.splice(index, 1);
    }
  }

}
