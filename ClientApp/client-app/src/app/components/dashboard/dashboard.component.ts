import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import { skip } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  customers:any = [];
  pageSize:number = 10;
  pageIndex:number = 1;
  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.customers = [
      {id: 1, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 2, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 3, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 4, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 5, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 6, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 7, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 8, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 9, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 10, name: "duy", country: "HN", company: "o2", role:"dev" },
      {id: 11, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 12, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 13, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 14, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 15, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 16, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 17, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 18, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 19, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 20, name: "duy2", country: "HN", company: "o2", role:"dev" },
      {id: 21, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 22, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 23, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 24, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 25, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 26, name: "duy3", country: "HN", company: "o2", role:"dev" },
      {id: 27, name: "duy3", country: "HN", company: "o2", role:"dev" },
    ]
    const currentUser:any = JSON.parse(this._userService.getCurrentUser());
    this.getData();
    //this.getUserRole(currentUser.userId);
  }
  skip(number){
    const data = this.customers.filter((e,i) => i >= number).slice(number, this.pageSize);
    console.log(data)
  }
  getData(){
    let skip = (this.pageSize * (this.pageIndex - 1));
    this.skip(skip);
  }
  getUserRole(userId: any){
    this._userService.getRoles(userId).subscribe(n => {
    })
  }
}
