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
variable cases
forth definitions internals

: CASE ( n -- ) cases @  0 cases ! ; immediate
: ENDCASE   cases @ for aft postpone then then next
            cases ! postpone drop ; immediate
: OF ( n -- ) postpone over postpone = postpone if ; immediate
: ENDOF   1 cases +! postpone else ; immediate

forth definitions
