import {Component, inject, OnInit, signal} from '@angular/core';
import {MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {Algorithm} from '../../shared/models/algorithm';
import {AlgorithmService} from '../../core/services/algorithm.service';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {FormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-algorithm',
  imports: [
    MatCardHeader,
    MatCard,
    MatCardContent,
    MatFormField,
    MatInput,
    FormsModule,
    MatCardActions,
    NgIf,
    MatButton,
    MatLabel,
    MatCardTitle
  ],
  templateUrl: './algorithm.component.html',
  standalone: true,
  styleUrl: './algorithm.component.scss'
})
export class AlgorithmComponent implements OnInit{
  algorithm = signal<Algorithm | undefined>(undefined);

  editableAlgorithm: Algorithm = { title: '', description: '' };

  algorithmService = inject(AlgorithmService);
  isEditing = false;

  ngOnInit() {
    this.loadAlgorithm();
  }

  editAlgorithm(): void {
    this.isEditing = true;
  }

  private loadAlgorithm(): void {
    this.algorithmService.getAlgorithm().subscribe({
      next: (data) => {
        this.algorithm.set(data);
        this.editableAlgorithm = { ...data };
      },
      error: (err) => {
        console.error('Failed to fetch algorithm:', err);
      }
    });
  }

  saveChanges(): void {
    if (!this.editableAlgorithm.title || !this.editableAlgorithm.description) {
      console.warn('Both title and description are required.');
      return;
    }

    this.algorithmService.updateAlgorithm(this.editableAlgorithm).subscribe({
      next: () => {
        console.log('Algorithm updated successfully');
        this.loadAlgorithm()
        this.isEditing = false;
      },
      error: (err) => {
        console.error('Failed to update algorithm:', err);
      }
    });
  }

  cancelEditing(): void {
    this.editableAlgorithm = { ...this.algorithm()! };
    this.isEditing = false;
  }
}
