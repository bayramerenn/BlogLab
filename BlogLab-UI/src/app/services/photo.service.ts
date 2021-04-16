import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Photo } from '../models/photo/photo.model';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  
  apiUrl:string;
  constructor(
    private http: HttpClient
  ) {
    this.apiUrl = environment.wepApi + "/Photo";
  }

  create(model: FormData) : Observable<Photo> {
    return this.http.post<Photo>(`${this.apiUrl}`, model);
  }

  getByApplicationUserId() : Observable<Photo[]> {
    return this.http.get<Photo[]>(`${this.apiUrl}`);
  }

  get(photoId: number) : Observable<Photo> {
    return this.http.get<Photo>(`${this.apiUrl}/${photoId}`);
  }

  delete(photoId: number) : Observable<number>{
    return this.http.delete<number>(`${this.apiUrl}/${photoId}`);
  }
}
