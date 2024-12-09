import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';


const SHAKE_DURATION_SECONDS = 2.0;
const BOUNCE_DURATION_SECONDS = 4.0;
const TADA_DURATION_SECONDS = 3.0;
const ROTATE_LEFT_DURATION_SECONDS = 2;
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations:[
    trigger('ngShake', [
      transition('* => *', useAnimation(shakeX, {
        params: { timing: SHAKE_DURATION_SECONDS  }
      }))
    ]),
    trigger('ngBounce', [
      transition('* => *', useAnimation(bounce, {
        params: { timing: BOUNCE_DURATION_SECONDS  }
      }))
    ]),
    trigger('ngTada', [
      transition('* => *', useAnimation(tada, {
        params: { timing: TADA_DURATION_SECONDS  }
      }))
    ])
  ]
})
export class AppComponent {
  title = 'ngAnimations';
  ng_shake = 0;
  ng_bounce = 0;
  ng_tada = 0;
  css_rotateleft = false;
  constructor() {
  }

  startAnimations(){
    const squares = document.querySelectorAll('.square');
  squares[0]?.classList.add('shake');
  squares[1]?.classList.add('bounce');
  squares[2]?.classList.add('tada');
  }

  async animerunefois(){
    this.ng_shake++;
    await lastValueFrom(timer(SHAKE_DURATION_SECONDS * 1000));
    this.ng_bounce++;
    await lastValueFrom(timer(BOUNCE_DURATION_SECONDS * 1000));
    this.ng_tada++;
    await lastValueFrom(timer(TADA_DURATION_SECONDS * 1000));

  }

  playLoop_Angular_3(){
    this.playShake();
  }
  playShake(){
    this.ng_shake++;
    setTimeout(() => {
      this.playbounce();
    },SHAKE_DURATION_SECONDS* 1000);
  }
  playbounce(){
    this.ng_bounce++;;
    setTimeout(() => {
      this.playtada();
    }, BOUNCE_DURATION_SECONDS* 1000);
  }
  playtada(){
    this.ng_tada++;
    setTimeout(() => {
      this.playShake();
    }, TADA_DURATION_SECONDS* 1000);
  }

  rotateleft(){
    this.css_rotateleft = true;
    setTimeout(() => {
      this.css_rotateleft = false;
    }, ROTATE_LEFT_DURATION_SECONDS * 1000);
  }

}
