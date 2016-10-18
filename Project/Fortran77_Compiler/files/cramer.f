! This program uses Cramer's rule to solve a system of 3 linear equations
! with 3 unknows.
!    
! You can try it with this input:
!
!      5.4 -1  3  2
!        1  5 -2  3
!        3  2  4  6
    
      program cramer
            
      real det, d, x 
      real a(3, 3)
      real b(3)
      real ai(3, 3)
      integer i
      
      write(*, *) 'This program will find the values of x1, x2, and x3'
      write(*, *) 'given a system of 3 linear equations:'
      write(*, *) 
      write(*, *) '   a1*x1 + b1*x2 + c1*x3 = d1'
      write(*, *) '   a2*x1 + b2*x2 + c2*x3 = d2'
      write(*, *) '   a3*x1 + b3*x2 + c3*x3 = d3'
      write(*, *)
      do 10 i = 1, 3
          write(*, *) 'Type the values of a, b, c, and d for row', i
          read(*,*) a(i, 1), a(i, 2), a(i, 3), b(i)
10    continue

      write(*, *)
      write(*, *) 'Results:'
      d = det(a)      
      do 20 i = 1, 3
          call replacecolumn(a, b, i, ai)
          x = det(ai) / d
          write(*, *) 'x', i, ' =', x
20    continue  
            
      stop
      end
      
!-------------------------------------------------------------------------------
! Compute the determinant of 3x3 matrix m.
    
      real function det(m)
      
      real m(3, 3)
      
      det = m(1, 1) * (m(2, 2) * m(3, 3) - m(3, 2) * m(2, 3)) -
     +      m(1, 2) * (m(2, 1) * m(3, 3) - m(3, 1) * m(2, 3)) +
     +      m(1, 3) * (m(2, 1) * m(3, 2) - m(3, 1) * m(2, 2))
     
      return
      end

!-------------------------------------------------------------------------------
! Copies into ac the contents of matrix a, but replaces column c with the 
! contents of vector b.    
      subroutine replacecolumn(a, b, c, ac)
      
      real a(3, 3), b(3), ac(3, 3)
      integer c, i, j
      
      do 20 i = 1, 3
          do 10 j = 1, 3 
              if (j .eq. c) then
                  ac(i, j) = b(i)
              else
                  ac(i, j) = a(i, j)
              endif              
10        continue
20    continue      
                        
      return
      end
      
