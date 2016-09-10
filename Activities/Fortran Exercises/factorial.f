      program factorial
      integer x, result

      write(*, *) 'Enter a number:'
      read(*, *) x

      result = 1
250   if (x .gt. 1) then
        result = result * x
        x = x - 1
        goto 250
      endif

      write(*, *) 'The factorial is: ', result

      stop
      end
