import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';

@Component({
  selector: 'app-project-flow',
  templateUrl: './project-flow.component.html',
  styleUrls: ['./project-flow.component.css']
})
export class ProjectFlowComponent implements OnInit {
  projectId: any;
  packages: any = [];
  activeIndex: number = 0;
  constructor(
    private route: ActivatedRoute,
    private _biddingService: BiddingService
  ) { }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    this.getPackageByProjectId();
  }
  openCreatePackageForm() {

  }
  getPackageByProjectId() {
    this._biddingService.getPackageProjectId(this.projectId).subscribe(response => {
      if (response.success) {
        const packageConverted = response.responseData.map(n => ({
          label: n.biddingPackageName,
          order: n.order,
          command: (event: any) => {
            this.activeIndex = n.order;
          }
        }))
        this.packages = packageConverted;
      }
    })
  }
}
