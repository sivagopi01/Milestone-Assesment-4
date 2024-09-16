import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;  // Error message for failed login

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    const loginUrl = `{this.apiUrl}/login`;  // Correctly interpolate the API URL
    this.authService.login(loginUrl, this.loginForm.value).subscribe({
        next: (response) => {
            console.log('Login successful', response);
            this.errorMessage = null;  // Clear error message on success
        },
        error: (err) => {
            console.error('Login failed', err);
            this.errorMessage = 'Invalid username or password';  // Set error message on failure
        }
    });
  }
}