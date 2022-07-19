\ Copyright 2022 Bradley D. Nelson
\
\ Licensed under the Apache License, Version 2.0 (the "License");
\ you may not use this file except in compliance with the License.
\ You may obtain a copy of the License at
\
\     http://www.apache.org/licenses/LICENSE-2.0
\
\ Unless required by applicable law or agreed to in writing, software
\ distributed under the License is distributed on an "AS IS" BASIS,
\ WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
\ See the License for the specific language governing permissions and
\ limitations under the License.

vocabulary web   web definitions

: jseval! ( a n index -- ) 0 call ;

r|
(function(sp) {
  var n = i32[sp>>2]; sp -= 4;
  var a = i32[sp>>2]; sp -= 4;
  var text = GetString(a, n);
  eval(text);
  return sp;
})
| 1 jseval!
: jseval ( a n -- ) 1 call ;

r|
context.inbuffer = [];
context.outbuffer = '';
if (!globalObj.write) {
  context.screen = document.getElementById('ueforth');
  if (context.screen === null) {
    context.screen = document.createElement('div');
    document.body.appendChild(context.screen);
  }
  context.filler = document.createElement('div');
  document.body.insertBefore(context.filler, document.body.firstChild);
  context.canvas = document.createElement('canvas');
  context.canvas.width = 1000;
  context.canvas.height = 1000;
  context.canvas.style.width = '1px';
  context.canvas.style.height = '1px';
  context.canvas.style.top = 0;
  context.canvas.style.left = 0;
  context.canvas.style.position = 'fixed';
  context.canvas.style.backgroundColor = '#000';
  context.screen.appendChild(context.canvas);
  context.ctx = context.canvas.getContext('2d');
  context.terminal = document.createElement('pre');
  context.screen.appendChild(context.terminal);
  context.mode = 1;
  function setMode(mode) {
    if (context.mode === mode) {
      return ;
    }
    if (mode) {
      context.filler.style.display = '';
      context.canvas.style.display = '';
    } else {
      context.filler.style.display = 'none';
      context.canvas.style.display = 'none';
    }
    context.mode = mode;
  }
  context.setMode = setMode;
  function Resize() {
    var width = window.innerWidth;
    var theight = Math.max(120, Math.floor(window.innerHeight / 6));
    var height = window.innerHeight - theight;
    if (width === context.width && height === context.height) {
      return;
    }
    context.canvas.style.width = width + 'px';
    context.canvas.style.height = height + 'px';
    context.filler.style.width = '1px';
    context.filler.style.height = height + 'px';
    context.width = width;
    context.height = height;
  }
  function Draw() {
    Resize();
    context.ctx.fillStyle = '#000';
    context.ctx.fillRect(0, 0, context.canvas.width, context.canvas.height);
  }
  window.onresize = function(e) {
    Resize();
  };
  window.onkeypress = function(e) {
    context.inbuffer.push(e.keyCode);
  };
  window.onkeydown = function(e) {
    if (e.keyCode == 8) {
      context.inbuffer.push(e.keyCode);
    }
  };
  setMode(0);
  Draw();
}
| jseval

r|
(function(sp) {
  var n = i32[sp>>2]; sp -= 4;
  var a = i32[sp>>2]; sp -= 4;
  if (globalObj.write) {
    var text = GetString(a, n);
    write(text);
  } else {
    for (var i = 0; i < n; ++i) {
      var ch = u8[a + i];
      if (ch == 12) {
        context.outbuffer = '';
      } else if (ch == 8) {
        context.outbuffer = context.outbuffer.slice(0, -1);
      } else if (ch == 13) {
      } else {
        context.outbuffer += String.fromCharCode(ch);
      }
    }
    context.terminal.innerText = context.outbuffer + String.fromCharCode(0x2592);
    window.scrollTo(0, document.body.scrollHeight);
  }
  return sp;
})
| 2 jseval!
: web-type ( a n -- ) 2 call yield ;
' web-type is type

r|
(function(sp) {
  if (globalObj.readline && !context.inbuffer.length) {
    var line = readline();
    for (var i = 0; i < line.length; ++i) {
      context.inbuffer.push(line.charCodeAt(i));
    }
    context.inbuffer.push(13);
  }
  if (context.inbuffer.length) {
    sp += 4; i32[sp>>2] = context.inbuffer.shift();
  } else {
    sp += 4; i32[sp>>2] = 0;
  }
  return sp;
})
| 3 jseval!
: web-key ( -- n ) begin yield 3 call dup if exit then drop again ;
' web-key is key

r|
(function(sp) {
  if (globalObj.readline) {
    sp += 4; i32[sp>>2] = -1;
    return sp;
  }
  sp += 4; i32[sp>>2] = context.inbuffer.length ? -1 : 0;
  return sp;
})
| 4 jseval!
: web-key? ( -- f ) yield 4 call ;
' web-key? is key?

r|
(function(sp) {
  var val = i32[sp>>2]; sp -= 4;
  if (globalObj.quit) {
    quit(val);
  } else {
    Init();
  }
  return sp;
})
| 5 jseval!
: terminate ( n -- ) 5 call ;

r|
(function(sp) {
  if (globalObj.write) {
    sp += 4; i32[sp>>2] = 0;  // Disable echo.
  } else {
    sp += 4; i32[sp>>2] = -1;  // Enable echo.
  }
  return sp;
})
| 6 jseval! 6 call echo !

r|
(function(sp) {
  var mode = i32[sp>>2]; sp -= 4;
  if (globalObj.write) {
    return sp;
  }
  context.setMode(mode);
  return sp;
})
| 7 jseval!

r|
(function(sp) {
  var c = i32[sp>>2]; sp -= 4;
  var h = i32[sp>>2]; sp -= 4;
  var w = i32[sp>>2]; sp -= 4;
  var y = i32[sp>>2]; sp -= 4;
  var x = i32[sp>>2]; sp -= 4;
  if (globalObj.write) {
    return sp;
  }
  function HexDig(n) {
    return ('0' + n.toString(16)).slice(-2);
  }
  context.ctx.fillStyle = '#' + HexDig((c >> 16) & 0xff) +
                                HexDig((c >> 8) & 0xff) +
                                HexDig(c & 0xff);
  context.ctx.fillRect(x, y, w, h);
  return sp;
})
| 8 jseval!

r|
(function(sp) {
  var h = i32[sp>>2]; sp -= 4;
  var w = i32[sp>>2]; sp -= 4;
  if (globalObj.write) {
    return sp;
  }
  context.canvas.width = w;
  context.canvas.height = h;
  return sp;
})
| 9 jseval!

r|
(function(sp) {
  if (globalObj.write) {
    sp += 4; i32[sp>>2] = 1;
    sp += 4; i32[sp>>2] = 1;
    return sp;
  }
  sp += 4; i32[sp>>2] = context.width;
  sp += 4; i32[sp>>2] = context.height;
  return sp;
})
| 10 jseval!

forth definitions web

: bye   0 terminate ;
: page   12 emit ;
: gr   1 7 call ;
: text   0 7 call ;
$ffffff value color
: box ( x y w h -- ) color 8 call ;
: window ( w h -- ) 9 call ;
: viewport@ ( -- w h ) 10 call ;

forth definitions