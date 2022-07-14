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

internals definitions
( TODO: Figure out why this has to happen so late. )
transfer internals-builtins
forth definitions internals
( Bring a forth to the top of the vocabulary. )
: ok   ." uEforth" raw-ok ;

: web-type   0 call ;  ' web-type is type
: web-key   yield 0 ;  ' web-key is key
: web-key?   yield 0 ;  ' web-key? is key?

transfer forth
forth
ok
