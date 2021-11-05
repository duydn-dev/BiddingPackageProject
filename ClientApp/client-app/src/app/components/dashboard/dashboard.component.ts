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
    const currentUser:any = JSON.parse(this._userService.getCurrentUser());
    this.getData();
    //this.getUserRole(currentUser.userId);
  }
  skip(number){
    const data = this.customers.filter((e,i) => i >= number).slice(number, this.pageSize);
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
