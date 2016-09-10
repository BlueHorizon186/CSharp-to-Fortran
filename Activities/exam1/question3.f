!     Ivan David Diaz Sanchez
!     A01371166

      program question3
      real taylor

      write(*, *) taylor(1, 1)
      write(*, *) taylor(2, 1)
      write(*, *) taylor(5, 1)
      write(*, *) taylor(10, 1)
      write(*, *) taylor(10, 5)

      stop
      end

!----------------------------------------------------------
      real function taylor(n, x)
      integer n, x

      real sum, factorial
!     Your code goes here.
      sum = 1

      do 150 i = 1, n
          sum = sum + ((x**i) / factorial(i))
150   continue

      taylor = sum
      return
      end

!----------------------------------------------------------
      real function factorial(x)
      integer x

      integer temp
      temp = x
      result = 1

250   if (temp .gt. 1) then
          result = result * temp
          temp = temp - 1
          goto 250
      endif

      factorial = result
      return
      end
