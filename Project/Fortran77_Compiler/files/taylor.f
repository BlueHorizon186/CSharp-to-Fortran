! Computes Taylor polynomials.

      program taylorprog
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
      integer n, x, i, fact
      real s
      s = 0
      do 10 i = 0, n
          s = s + real(x ** i) / fact(i)
10    continue
      taylor = s
      return
      end

!----------------------------------------------------------
      integer function fact(n)
      integer n, i, r
      r = 1
      do 10 i = 1, n
          r = r * i
10    continue
      fact = r
      return
      end
