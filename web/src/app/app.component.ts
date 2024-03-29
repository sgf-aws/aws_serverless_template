import { Component, OnInit } from '@angular/core';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  version:string;

  constructor(private apiService: ApiService){}

  ngOnInit(){
    this.apiService.getVersion().subscribe(versionInfo => {
      this.version = versionInfo.version;
    });
  }
}
