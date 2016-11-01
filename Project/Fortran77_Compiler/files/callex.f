! This program demonstrates the call-by-reference mechanism.

      program callex
      integer m, n

      m = 1
      n = 2

      call iswap(m, n)
      write(*, *) m, n

      call iswap(m + 0, n * 1)
      write(*, *) m, n

      stop
      end

!-------------------------------------------------------------------------------
! Swaps two integer variables

      subroutine iswap(a, b)
      integer a, b
      integer tmp

      tmp = a
      a = b
      b = tmp

      return
      end
!-------------------------------------------------------------------------------
