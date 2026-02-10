import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { CatsApiService } from './../core/cats-api.service';

import { CatBreed } from '../models/catBreed.model';
import { CatImage } from '../models/catImage.model';

import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageModule } from 'primeng/message';
import { ImageModule } from 'primeng/image';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    InputNumberModule,
    ButtonModule,
    CardModule,
    ProgressSpinnerModule,
    MessageModule,
    ImageModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private readonly api = inject(CatsApiService);

  breeds: CatBreed[] = [];
  selectedBreedId: string | null = null;

  limit = 10;

  images: CatImage[] = [];

  loadingBreeds = false;
  loadingImages = false;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.loadBreeds();
  }

  loadBreeds(): void {
    this.loadingBreeds = true;
    this.errorMessage = null;

    this.api.getBreeds()
      .pipe(finalize(() => (this.loadingBreeds = false)))
      .subscribe({
        next: (data) => {
          this.breeds = data;
          if (!this.selectedBreedId && data.length > 0) {
            this.selectedBreedId = data[0].id;
          }
        },
        error: () => {
          this.errorMessage = 'No se pudieron cargar las razas.';
        }
      });
  }

  search(): void {
    this.errorMessage = null;
    this.images = [];

    const breedId = this.selectedBreedId?.trim();
    const limit = Number(this.limit);

    if (!breedId) {
      this.errorMessage = 'Selecciona una raza.';
      return;
    }

    if (!Number.isFinite(limit) || limit < 1 || limit > 50) {
      this.errorMessage = 'El límite debe estar entre 1 y 50.';
      return;
    }

    this.loadingImages = true;

    this.api.getImages(breedId, limit)
      .pipe(finalize(() => (this.loadingImages = false)))
      .subscribe({
        next: (imgs) => {
          this.images = imgs;
          if (imgs.length === 0) {
            this.errorMessage = 'No se encontraron imágenes para esa raza.';
          }
        },
        error: (err) => {
          const apiMsg = err?.error?.message;
          this.errorMessage = apiMsg ? String(apiMsg) : 'Error al buscar imágenes.';
        }
      });
  }

  trackById(_: number, item: CatImage): string {
    return item.id;
  }
}
