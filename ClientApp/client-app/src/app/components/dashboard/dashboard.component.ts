import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import { ProjectService } from 'src/app/services/project/project.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  customers: any = [];
  pageSize: number = 10;
  pageIndex: number = 1;
  chartOptions: any;
  data: any;
  option: any = {
    legend: {
      labels: {
        fontColor: 'white'
      }
    }
  }
  staticalData:any = {};
  constructor(
    private _userService: UserService,
    private _projectService: ProjectService
  ) { }

  ngOnInit(): void {
    this.getStatistical();
  }
  getUserRole(userId: any) {
    this._userService.getRoles(userId).subscribe(n => {
    })
  }
  getStatistical() {
    this._projectService.getStatistical().subscribe(response => {
      if (response.success) {
        this.staticalData = response.responseData;
        console.log(this.staticalData)
        this.data = {
          labels: ['Đã hoàn thành', 'Chưa hoàn thành'],
          datasets: [
            {
              data: [response.responseData.ratioProjectComplete, response.responseData.ratioProjectNotComplete],
              backgroundColor: [
                "#FF6384",
                "#36A2EB"
              ],
              color: "#FF6384",
            }
          ]
        }
      }
    })
  }
}
