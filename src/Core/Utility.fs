namespace Lacjam.Core

    module Utility =
         
         ///http://books.google.com.au/books?id=MH3-T2jGFsEC&pg=PA22&lpg=PA22&dq=daniel+mohl+nullcheck&source=bl&ots=a8vv40Iklg&sig=j21km-HFd3Jan9uqoIPIyBLzmvU&hl=en&sa=X&ei=Nma-UoGcO4XMkQXi8YDwDQ&ved=0CEMQ6AEwAw#v=onepage&q=daniel%20mohl%20nullcheck&f=false
         let NullCheck = function
            | v when v <> null -> Some v
            | _ -> None


