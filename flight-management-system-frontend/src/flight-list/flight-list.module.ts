import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightListComponent } from './flight-list.component';
import { HttpClientModule } from '@angular/common/http';
import {MatButtonModule} from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogsModule } from 'src/dialogs/dialogs.module';
import { ServicesModule } from 'src/services/services.module';

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
    ServicesModule
  ],
  exports: [
    FlightListComponent
  ]
})
export class FlightListModule { }
