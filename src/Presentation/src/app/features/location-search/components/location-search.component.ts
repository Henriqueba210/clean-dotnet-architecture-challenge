import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { take } from "rxjs";
import { LocationService } from "../../../core/services/location.service";
import { AppLocation } from "../models/location.model";
import { ErrorSnackbarComponent } from './error-snackbar.component';
import { animate, style, transition, trigger } from '@angular/animations';
import { LocationCardComponent } from './location-card/location-card.component';

@Component({
  selector: 'app-location-search',
  templateUrl: './location-search.component.html',
  styleUrls: ['./location-search.component.scss'],
  standalone: true,
  animations: [
    trigger('cardAnimation', [
      transition(':enter', [
        style({ transform: 'scale(0.8)', opacity: 0 }),
        animate('300ms ease-out', style({ transform: 'scale(1)', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ transform: 'scale(0.8)', opacity: 0 }))
      ])
    ]),
    trigger('listAnimation', [
      transition(':enter', [
        style({ height: 0, opacity: 0 }),
        animate('300ms ease-out', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ height: 0, opacity: 0 }))
      ])
    ])
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatListModule,
    MatSnackBarModule,
    MatIconModule,
    LocationCardComponent
  ]
})
export class LocationSearchComponent {
  postcode = '';
  currentLocation: AppLocation | null = null;
  searchHistory: AppLocation[] = [];

  constructor(
    private locationService: LocationService,
    private snackBar: MatSnackBar
  ) {}

  search(): void {
    this.locationService.getLocation(this.postcode)
      .pipe(take(1))
      .subscribe({
        next: location => {
          this.currentLocation = location;
          this.updateHistory(location);
        },
        error: () => {
          this.snackBar.openFromComponent(ErrorSnackbarComponent, {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['error-snackbar'],
            data: { message: 'Location not found' }
          });
        }
      });
  }

  private updateHistory(location: AppLocation): void {
    this.searchHistory.unshift(location);
    if (this.searchHistory.length > 3) {
      this.searchHistory.pop();
    }
  }
}