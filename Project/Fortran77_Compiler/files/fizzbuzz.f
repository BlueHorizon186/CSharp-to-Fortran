! This program prints the numbers from 1 to 100, but for multiples
! of three it prints "fizz" instead of the number and for multiples
! of five it prints "buzz". For numbers which are multiples of both three 
! and five it prints "fizzbuzz".
        
      program fizzbuzz
      
      integer rem, i

      do 10 i = 1, 100
      
          if (rem(i, 15) .eq. 0) then
              write(*, *) 'fizzbuzz'
          elseif (rem(i, 3) .eq. 0) then
              write(*, *) 'fizz'
          elseif (rem(i, 5) .eq. 0) then
              write(*, *) 'buzz'
          else
              write(*, *) i
          endif
          
10    continue

      stop
      end

!-------------------------------------------------------------------------------
! Returns the remainder of dividing its two integer params.
    
      integer function rem(a, b)
      
      integer a, b
      
      rem = a - (int(real(a) / real(b)) * b) 
      return 
      end
!-------------------------------------------------------------------------------

