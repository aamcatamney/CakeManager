import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private httpClient: HttpClient) {}
  files: string[] = [];
  title = 'CakeManager';
  ngOnInit() {
    this.httpClient.get<string[]>('/api/cake').subscribe(f => (this.files = f));
  }
  run(f: string) {
    this.httpClient
      .get('/api/cake/run', { params: { FilePath: f } })
      .subscribe();
  }
}
