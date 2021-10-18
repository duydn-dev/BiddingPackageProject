import { Component, OnInit } from '@angular/core';
import { MessageService, TreeNode } from 'primeng/api';
import { UserService } from 'src/app/services/user/user.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-decentralization',
  templateUrl: './decentralization.component.html',
  styleUrls: ['./decentralization.component.css']
})
export class DecentralizationComponent implements OnInit {
  selectedFiles: any = [];
  arr:any = [];
  data: TreeNode[] = [];
  userId: any;
  constructor(
    private route: ActivatedRoute,
    private _messageService: MessageService,
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId');
    this.getRoleAndRoleSelected();
  }
  getRoleAndRoleSelected(){
    this._userService.getRoleAndRoleSelected(this.userId).subscribe(response => {
      this.data = response.responseData.listRole;
      this.selectedFiles = response.responseData.selectedIds;
      this.arr = Array.from(response.responseData.selectedIds);
      this.checkNode(response.responseData.listRole, response.responseData.selectedIds);
    })
  }
  checkNode(nodes:TreeNode[], str:string[]) {
    nodes.forEach(node => {
      if(str.includes(node.data)) {
        this.selectedFiles.push(node);
      }

      if(node.children != undefined){
        node.children.forEach(child => {
          if(str.includes(child.data) && !str.includes(node.data)) {
            node.partialSelected = true;
            child.parent = node;
          }
          if(str.includes(node.data)){
            child.parent = node;
            str.push(child.data);
          }
        });
      }else{
        return;
      }

      this.checkNode(node.children, str);

      node.children.forEach(child => {
        if(child.partialSelected) {
          node.partialSelected = true;
        }
      });
    });
  }
  onNodeSelect(event:any){
    if(this.arr.length >= 0){
      if(event.node && this.arr.indexOf(event.node.data) === -1){
        this.arr.push(event.node.data)
      }
    }
  }
  onNodeUnselect(event:any){
    if(this.arr.length > 0){
      const i = this.arr.indexOf(event.node.data);
      this.arr.splice(i, 1);
    }
  }
  save(){
    const obj = {
      userId: this.userId,
      roleIds: this.arr
    }
    this._userService.updateRole(obj).subscribe(response => {
      if(response.success){
        this._messageService.add({ severity: 'success', summary: 'Thành công', detail: "Cập nhật quyền thành công !" });
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
      }
    })
  }
}
