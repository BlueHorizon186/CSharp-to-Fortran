! Computes and prints the factorials of numbers from 0 to 10. Factorials are
! computed using iterative and recursive functions. 
        
      program factorials
      
      integer factit, factrec, i
      
      write(*, *) 'Iterative factorial'
      do 10 i = 0, 10          
          write(*,*) i, factit(i)      
10    continue      

      write(*, *)
      write(*, *) 'Recursive factorial'
      do 20 i = 0, 10          
          write(*,*) i, factrec(i)      
20    continue
            
      stop
      end
      
!-------------------------------------------------------------------------------
! Iterative version of factorial.
    
      integer function factit(n)
      
      integer n, r, i 
      
      r = 1
      
      do 10 i = 2, n
          r = r * i
10    continue

      factit = r
      
      return 
      end
      
!-------------------------------------------------------------------------------
! Recursive version of factorial.
    
      integer function factrec(n)
      
      integer n 
      
      if (n .eq. 0) then
          factrec = 1
      else
          factrec = n * factrec(n - 1)
      endif
      
      return 
      end      
      
!-------------------------------------------------------------------------------      

