import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApplicationUserLogin } from 'src/app/models/account/application-user-login.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm : FormGroup;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username:[null,[
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(20)
      ]],
      password :[null,[
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(50)
      ]]
    })
  }

  isTouched(field:string){
    return this.loginForm.get(field)?.errors;
  }

  hasErrors(field:string){
    return this.loginForm.get(field)?.errors;
  }

  public hasError(field:string,error:string){
    return !!this.loginForm.get(field)?.hasError(error);
  }

  onSubmit(){
    if (this.loginForm.valid) {
      let applicationuserLogin :ApplicationUserLogin = 
        new ApplicationUserLogin(
          this.loginForm.get('username')?.value,
          this.loginForm.get('password')?.value,
        )

      this.accountService.login(applicationuserLogin).subscribe(() => {
        this.router.navigateByUrl("/dashboard")
      })
    }
  }
}
