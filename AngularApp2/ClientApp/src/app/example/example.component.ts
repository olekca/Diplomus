import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'example',
  templateUrl: './example.component.html'
})

export class ExampleComponent {
  public currentCount = 1;
  public txt:string = 'q';

  
  constructor(private http: HttpClient) { }

  public incrementCounter() {
    this.currentCount++;
   // this.http.get('').subscribe({ next: (data: any) => this.txt = new theF(data.Fucc).Fucc });
    this.http.get<theF>('https://localhost:44482/weatherforecast/example').subscribe(result => {
      this.txt = result.Fucc;
    });

  }
}

export class theFa {
  constructor(public Fucc: string) { }
}
interface theF {
  Fucc:string
} 
