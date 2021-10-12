import { Component, OnInit } from '@angular/core';
declare var $:any;

@Component({
  selector: 'app-menu-left',
  templateUrl: './menu-left.component.html',
  styleUrls: ['./menu-left.component.css']
})
export class MenuLeftComponent implements OnInit {

  menuData:any[] = [];
  constructor() { }

  ngOnInit(): void {
    this.menuData = [
      {
        name: "Dashboard",
        link: "/",
        icon: "home",
      },
      {
        name: "Phòng họp",
        link: "/rooms",
        icon: "meeting_room"
      },
      {
        name: "Hệ thống",
        icon: "settings",
        childs:[
          {name: "Người dùng", link: "/users"},
          {name: "Quyền hạn", link: "/role"},
        ]
      },
    ]
  }
  
  menuClick(e){
    if($(e.target).next().attr('class') == "mdl-navigation"){
      $(e.target).parent().parent().toggleClass('sub-navigation--show');
      $(e.target).toggleClass('mdl-navigation__link--current');
    }
  }
}
