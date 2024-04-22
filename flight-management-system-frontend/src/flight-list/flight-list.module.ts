import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightListComponent } from './flight-list.component';
import { HttpClientModule } from '@angular/common/http';
import {MatButtonModule} from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogsModule } from 'src/dialogs/dialogs.module';

@NgModule({
  declarations: [
    FlightListComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    MatButtonModule,
    DialogsModule,
    MatDialogModule,

  ],
  exports: [
    FlightListComponent
  ]
})
export class FlightListModule { }
