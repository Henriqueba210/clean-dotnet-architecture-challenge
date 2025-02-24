import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-snackbar',
  template: `
    <div class="snackbar-content">
      <mat-icon>warning</mat-icon>
      <span>Location not found</span>
    </div>
  `,
  styles: [`
    .snackbar-content {
      display: flex;
      align-items: center;
      gap: 12px;
      font-size: 16px;
      padding: 8px 4px;
    }
    mat-icon {
      color: #ff9800;
      font-size: 24px;
      height: 24px;
      width: 24px;
    }
  `],
  standalone: true,
  imports: [MatIconModule, CommonModule]
})
export class ErrorSnackbarComponent {}