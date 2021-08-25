import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class MenuComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    
  }

  clickedMenuItem(event: MouseEvent) {
    if(this.isClickedLink(event.target)) {
      //this.collapseMenu();
    }
  }

  isClickedLink(eventTarget: EventTarget) : boolean {
    return (eventTarget as Element).className === "top-menu-items-link";
  }

  collapseMenu() {
    (document.getElementById("top-menu-toggle") as HTMLInputElement).checked = false;
  }

}
