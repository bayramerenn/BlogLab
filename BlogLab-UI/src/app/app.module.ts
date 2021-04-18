import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BsDropdownModule} from 'ngx-bootstrap/dropdown'
import {CollapseModule} from 'ngx-bootstrap/collapse'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {TypeaheadModule} from 'ngx-bootstrap/typeahead'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SummaryPipe } from './pipes/summary.pipe';
import { BlogComponent } from './components/blog-components/blog/blog.component';
import { BlogCardComponent } from './components/blog-components/blog-card/blog-card.component';
import { BlogEditComponent } from './components/blog-components/blog-edit/blog-edit.component';
import { BlogsComponent } from './components/blog-components/blogs/blogs.component';
import { FamousBlogsComponent } from './components/blog-components/famous-blogs/famous-blogs.component';
import { CommentBoxComponent } from './components/comment-components/comment-box/comment-box.component';
import { CommentSystemComponent } from './components/comment-components/comment-system/comment-system.component';
import { CommentsComponent } from './components/comment-components/comments/comments.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { PhotoAlbumComponent } from './components/photo-album/photo-album.component';
import { RegisterComponent } from './components/register/register.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { ToastrModule as ToastsModule } from 'ngx-toastr';
import { NotFoundComponent } from './components/not-found/not-found.component';



@NgModule({
  declarations: [
    AppComponent,
    SummaryPipe,
    BlogComponent,
    BlogCardComponent,
    BlogEditComponent,
    BlogsComponent,
    FamousBlogsComponent,
    CommentBoxComponent,
    CommentSystemComponent,
    CommentsComponent,
    DashboardComponent,
    HomeComponent,
    LoginComponent,
    NavbarComponent,
    PhotoAlbumComponent,
    RegisterComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ToastsModule.forRoot({
     positionClass: 'toast-bottom-right'
    }),
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    TypeaheadModule.forRoot()
  ],
  providers: [HttpClient,
    {provide:HTTP_INTERCEPTORS,useClass:JwtInterceptor,multi:true},
     {provide:HTTP_INTERCEPTORS,useClass:ErrorInterceptor,multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
