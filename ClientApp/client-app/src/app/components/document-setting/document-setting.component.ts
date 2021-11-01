import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DocumentService } from 'src/app/services/document/document.service';

@Component({
  selector: 'app-document-setting',
  templateUrl: './document-setting.component.html',
  styleUrls: ['./document-setting.component.css']
})
export class DocumentSettingComponent implements OnInit {
  projectId: any;
  packageDocument:any = [];
  constructor(
    private route: ActivatedRoute,
    private documentService : DocumentService
  ) { }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    this.documentService.getSettingDocument(this.projectId).subscribe(response => {
      if(response.success){
        this.packageDocument = response.responseData;
        console.log(this.packageDocument)
      }
    })
  }
}
