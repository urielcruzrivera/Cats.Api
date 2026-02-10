import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CatBreed } from '../models/catBreed.model';
import { CatImage } from '../models/catImage.model';


@Injectable({ providedIn: 'root' })
export class CatsApiService {
  private readonly baseUrl = 'https://localhost:7254/api/cats';

  constructor(private http: HttpClient) { }

  getBreeds(): Observable<CatBreed[]> {
    return this.http.get<CatBreed[]>(`${this.baseUrl}/breeds`);
  }

  getImages(breedId: string, limit: number): Observable<CatImage[]> {
    const params = new HttpParams()
      .set('breedId', breedId)
      .set('limit', limit);

    return this.http.get<CatImage[]>(`${this.baseUrl}/images`, { params });
  }
}
