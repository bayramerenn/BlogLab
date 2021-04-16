import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BlogCommentCreate } from '../models/blog-comment-create.model';
import { BlogComment } from '../models/blog-comment.model';

@Injectable({
  providedIn: 'root'
})
export class BlogCommentService {

  apiUrl: string;

  constructor(
    private http: HttpClient
  ) {
    this.apiUrl = environment.wepApi + "/blogcomment";
  }

  create(model: BlogCommentCreate): Observable<BlogComment> {
    return this.http.post<BlogComment>(this.apiUrl,model);
  }

  delete(blogCommentId:number):Observable<number>{
    return this.http.delete<number>(this.apiUrl + `/${blogCommentId}`)
  }

  getAll(blogId:number):Observable<BlogComment[]>{
    return this.http.get<BlogComment[]>(this.apiUrl + `/${blogId}`)
  }
}
